import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { CustomerWalletService } from '../../services/customer-wallet.service';
import { Wallet, Transaction, PaymentMethod, PaymentProvider, PaymentType, TransactionStatus, TransactionType } from '../../models/wallet.model';

@Component({
  selector: 'app-customer-wallet',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslateModule],
  templateUrl: './customer-wallet.component.html',
  styleUrls: ['./customer-wallet.component.css']
})
export class CustomerWalletComponent implements OnInit {
  wallet = signal<Wallet | null>(null);
  transactions = signal<Transaction[]>([]);
  paymentMethods = signal<PaymentMethod[]>([]);
  
  // Top Up Modal State
  topUpAmount = signal<number>(100);
  selectedPaymentMethodId = signal<number | null>(null);
  
  // Enums for template
  TransactionStatus = TransactionStatus;
  TransactionType = TransactionType;

  constructor(
    private walletService: CustomerWalletService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.walletService.getMyWallet().subscribe({
      next: (w) => this.wallet.set(w),
      error: () => this.toastr.error('Failed to load wallet')
    });

    this.walletService.getMyTransactions().subscribe({
      next: (t) => this.transactions.set(t),
      error: () => this.toastr.error('Failed to load transactions')
    });

    this.walletService.getMyPaymentMethods().subscribe({
      next: (pm) => this.paymentMethods.set(pm),
      error: () => this.toastr.error('Failed to load payment methods')
    });
  }

  initiateTopUp(): void {
    if (!this.selectedPaymentMethodId()) {
      this.toastr.warning('Please select a payment method');
      return;
    }
    
    // Using Paymob as default provider for now
    this.walletService.topUp(this.topUpAmount(), this.selectedPaymentMethodId()!, PaymentProvider.Paymob).subscribe({
      next: (res) => {
        // Redirect to payment gateway URL
        window.location.href = res.redirectUrl;
      },
      error: () => this.toastr.error('Failed to initiate top-up')
    });
  }

  deletePaymentMethod(id: number): void {
    if(confirm('Are you sure you want to delete this payment method?')) {
      this.walletService.deletePaymentMethod(id).subscribe({
        next: () => {
          this.toastr.success('Payment method deleted');
          this.loadData();
        },
        error: () => this.toastr.error('Failed to delete payment method')
      });
    }
  }

  getStatusBadgeClass(status: TransactionStatus): string {
    switch(status) {
      case TransactionStatus.Completed: return 'bg-success';
      case TransactionStatus.Pending: return 'bg-warning text-dark';
      case TransactionStatus.Failed: return 'bg-danger';
      case TransactionStatus.Reversed: return 'bg-secondary';
      default: return 'bg-primary';
    }
  }

  getTypeLabel(type: TransactionType): string {
    return TransactionType[type];
  }
}
