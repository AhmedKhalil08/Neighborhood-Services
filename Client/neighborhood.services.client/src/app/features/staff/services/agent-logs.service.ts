import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { PagedResult } from '../../../core/models/paged-result.model';

export type AgentLogType = 'Matching' | 'Pricing' | 'Booking' | 'QA' | 'Moderation' | 'Chatbot';

// Mirrors AgentLogDto (enums serialized as strings).
export interface AgentLog {
  id: number;
  agentType: AgentLogType;
  action: string;
  input: string;
  output: string;
  referenceType: string;
  referenceId: number | null;
  tokensUsed: number | null;
  createdAt: string;
}

export interface GetAgentLogsParams {
  type: AgentLogType;
  search?: string;
  page?: number;
  pageSize?: number;
}

@Injectable({ providedIn: 'root' })
export class AgentLogsService {
  private readonly api = inject(ApiService);

  /** GET /api/agentlogs — paged + filtered logs for one agent type (full-access staff only). */
  getLogs(params: GetAgentLogsParams): Observable<PagedResult<AgentLog>> {
    const q = new URLSearchParams();
    q.set('type', params.type);
    if (params.search?.trim()) q.set('search', params.search.trim());
    q.set('page', String(params.page ?? 1));
    q.set('pageSize', String(params.pageSize ?? 20));
    return this.api.get<PagedResult<AgentLog>>(`/agentlogs?${q.toString()}`);
  }
}
