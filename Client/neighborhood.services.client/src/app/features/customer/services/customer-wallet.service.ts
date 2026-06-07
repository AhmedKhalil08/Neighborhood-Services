import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/services/api.service';
import { Wallet, Transaction, PaymentMethod, TopUpResponse, PaymentProvider, PaymentType } from '../models/wallet.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerWalletService {

  constructor(private api: ApiService) {}

  getMyWallet(): Observable<Wallet> {
    return this.api.get<Wallet>('/wallets/me');
  }

  topUp(amount: number, paymentMethodId: number, provider: PaymentProvider): Observable<TopUpResponse> {
    return this.api.post<TopUpResponse>('/wallets/me/topup', { amount, paymentMethodId, provider });
  }

  getMyTransactions(): Observable<Transaction[]> {
    return this.api.get<Transaction[]>('/transactions/me');
  }

  getMyPaymentMethods(): Observable<PaymentMethod[]> {
    return this.api.get<PaymentMethod[]>('/paymentmethods/me');
  }

  addPaymentMethod(paymentType: PaymentType, paymentProvider: PaymentProvider, providerToken: string, lastFourDigits?: string, expiryMonth?: number, expiryYear?: number): Observable<PaymentMethod> {
    return this.api.post<PaymentMethod>('/paymentmethods', {
      paymentType,
      paymentProvider,
      providerToken,
      lastFourDigits,
      expiryMonth,
      expiryYear
    });
  }

  deletePaymentMethod(id: number): Observable<void> {
    return this.api.delete<void>(`/paymentmethods/${id}`);
  }
}
