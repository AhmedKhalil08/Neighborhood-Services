import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PagedResult } from '../../../../core/models/paged-result.model';
import { AgentLog, AgentLogType, AgentLogsService } from '../../services/agent-logs.service';

@Component({
  selector: 'app-agent-logs',
  standalone: true,
  imports: [FormsModule, DatePipe],
  template: `
    <div class="container-fluid py-3">
      <div class="d-flex flex-wrap align-items-center justify-content-between gap-2 mb-3">
        <div>
          <h1 class="h4 fw-bold mb-0">Agent Logs</h1>
          <small class="text-muted">Every AI decision, by agent — input, output, tokens.</small>
        </div>
      </div>

      <!-- Tabs: one per agent type -->
      <ul class="nav nav-pills flex-wrap gap-1 mb-3">
        @for (t of types; track t) {
          <li class="nav-item">
            <button class="nav-link" [class.active]="t === activeType()" type="button" (click)="selectType(t)">
              {{ t }}
            </button>
          </li>
        }
      </ul>

      <!-- Search -->
      <div class="d-flex flex-wrap gap-2 mb-3" style="max-width: 520px;">
        <input
          class="form-control"
          type="search"
          placeholder="Search input / output / action…"
          [ngModel]="search()"
          (ngModelChange)="search.set($event)"
          (keyup.enter)="applySearch()" />
        <button class="btn btn-primary" type="button" (click)="applySearch()">Search</button>
        @if (search()) {
          <button class="btn btn-outline-secondary" type="button" (click)="search.set(''); applySearch()">Clear</button>
        }
      </div>

      <div class="card border-0 shadow-sm">
        <div class="table-responsive">
          <table class="table table-sm table-hover align-middle mb-0">
            <thead class="table-light">
              <tr>
                <th style="width: 150px;">Time (UTC)</th>
                <th style="width: 120px;">Action</th>
                <th>Input</th>
                <th>Output</th>
                <th style="width: 80px;">Tokens</th>
                <th style="width: 130px;">Reference</th>
              </tr>
            </thead>
            <tbody>
              @if (loading()) {
                <tr><td colspan="6" class="text-center text-muted py-4">Loading…</td></tr>
              } @else if (error()) {
                <tr><td colspan="6" class="text-center text-danger py-4">Couldn't load logs. Try again.</td></tr>
              } @else if ((result()?.items?.length ?? 0) === 0) {
                <tr><td colspan="6" class="text-center text-muted py-4">No logs for {{ activeType() }}@if (search()) { matching "{{ search() }}"}.</td></tr>
              } @else {
                @for (log of result()!.items; track log.id) {
                  <tr class="log-row" (click)="open(log)" title="Click for full details">
                    <td class="text-nowrap"><small>{{ log.createdAt | date: 'yyyy-MM-dd HH:mm:ss' : 'UTC' }}</small></td>
                    <td><span class="badge text-bg-light">{{ log.action }}</span></td>
                    <td class="cell-clip" [title]="log.input">{{ log.input }}</td>
                    <td class="cell-clip" [title]="log.output">{{ log.output }}</td>
                    <td>{{ log.tokensUsed ?? '—' }}</td>
                    <td class="text-nowrap">
                      <small class="text-muted">{{ log.referenceType }}@if (log.referenceId) { #{{ log.referenceId }} }</small>
                    </td>
                  </tr>
                }
              }
            </tbody>
          </table>
        </div>

        <!-- Footer: count + pagination -->
        @if (result(); as r) {
          <div class="d-flex flex-wrap align-items-center justify-content-between gap-2 p-2 border-top">
            <small class="text-muted">{{ r.totalCount }} log(s) · page {{ r.page }} of {{ r.totalPages || 1 }}</small>
            <div class="btn-group btn-group-sm">
              <button class="btn btn-outline-secondary" type="button" [disabled]="!r.hasPrevious" (click)="goTo(r.page - 1)">‹ Prev</button>
              <button class="btn btn-outline-secondary" type="button" [disabled]="!r.hasNext" (click)="goTo(r.page + 1)">Next ›</button>
            </div>
          </div>
        }
      </div>

      <!-- Details modal -->
      @if (selected(); as log) {
        <div class="log-backdrop" (click)="close()">
          <div class="log-modal card shadow" (click)="$event.stopPropagation()">
            <div class="card-header d-flex align-items-center justify-content-between">
              <div class="d-flex align-items-center gap-2">
                <span class="badge text-bg-primary">{{ log.agentType }}</span>
                <span class="badge text-bg-light">{{ log.action }}</span>
              </div>
              <button class="btn-close" type="button" aria-label="Close" (click)="close()"></button>
            </div>
            <div class="card-body">
              <div class="row g-2 mb-3 small">
                <div class="col-sm-4"><span class="text-muted">Time (UTC):</span> {{ log.createdAt | date: 'yyyy-MM-dd HH:mm:ss' : 'UTC' }}</div>
                <div class="col-sm-4"><span class="text-muted">Tokens:</span> {{ log.tokensUsed ?? '—' }}</div>
                <div class="col-sm-4"><span class="text-muted">Reference:</span> {{ log.referenceType }}@if (log.referenceId) { #{{ log.referenceId }} }</div>
              </div>
              <h6 class="fw-bold mb-1">Input</h6>
              <pre class="log-pre">{{ log.input }}</pre>
              <h6 class="fw-bold mb-1 mt-3">Output</h6>
              <pre class="log-pre">{{ log.output }}</pre>
            </div>
          </div>
        </div>
      }
    </div>
  `,
  styles: [`
    .cell-clip {
      max-width: 360px;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }
    .log-row { cursor: pointer; }
    .log-backdrop {
      position: fixed;
      inset: 0;
      background: rgba(0, 0, 0, 0.5);
      display: flex;
      align-items: center;
      justify-content: center;
      z-index: 1060;
      padding: 1rem;
    }
    .log-modal {
      width: min(760px, 100%);
      max-height: 88vh;
      display: flex;
      flex-direction: column;
    }
    .log-modal .card-body { overflow: auto; }
    .log-pre {
      background: #f8f9fa;
      border: 1px solid #dee2e6;
      border-radius: 0.375rem;
      padding: 0.75rem;
      max-height: 32vh;
      overflow: auto;
      white-space: pre-wrap;
      word-break: break-word;
      font-size: 0.85rem;
      margin: 0;
    }
  `],
})
export class AgentLogsComponent implements OnInit {
  private readonly service = inject(AgentLogsService);

  readonly types: AgentLogType[] = ['Chatbot', 'Matching', 'Pricing', 'Booking', 'QA', 'Moderation'];
  readonly activeType = signal<AgentLogType>('Chatbot');
  readonly search = signal('');
  readonly page = signal(1);
  readonly pageSize = 20;

  readonly loading = signal(false);
  readonly error = signal(false);
  readonly result = signal<PagedResult<AgentLog> | null>(null);

  // The row whose full details are shown in the modal (null = closed).
  readonly selected = signal<AgentLog | null>(null);

  ngOnInit(): void {
    this.load();
  }

  open(log: AgentLog): void {
    this.selected.set(log);
  }

  close(): void {
    this.selected.set(null);
  }

  selectType(t: AgentLogType): void {
    if (t === this.activeType()) return;
    this.activeType.set(t);
    this.search.set('');
    this.page.set(1);
    this.load();
  }

  applySearch(): void {
    this.page.set(1);
    this.load();
  }

  goTo(p: number): void {
    const total = this.result()?.totalPages ?? 1;
    if (p < 1 || p > total) return;
    this.page.set(p);
    this.load();
  }

  private load(): void {
    this.loading.set(true);
    this.error.set(false);
    this.service
      .getLogs({ type: this.activeType(), search: this.search(), page: this.page(), pageSize: this.pageSize })
      .subscribe({
        next: (r) => {
          this.result.set(r);
          this.loading.set(false);
        },
        error: () => {
          this.error.set(true);
          this.loading.set(false);
        },
      });
  }
}
