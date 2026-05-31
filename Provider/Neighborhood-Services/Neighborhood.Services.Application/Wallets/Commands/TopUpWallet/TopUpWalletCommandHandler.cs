using MediatR;
using Neighborhood.Services.Application.Shared;
using Neighborhood.Services.Application.Transactions.Interfaces;
using Neighborhood.Services.Application.Wallets.DTOs;
using Neighborhood.Services.Application.Wallets.Interfaces;
using Neighborhood.Services.Domain.Transactions;
namespace Neighborhood.Services.Application.Wallets.Commands.TopUpWallet
{
    public class TopUpWalletCommandHandler : IRequestHandler<TopUpWalletCommand, WalletResponseDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TopUpWalletCommandHandler(IWalletRepository walletRepository, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<WalletResponseDto> Handle(TopUpWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId)
            ?? throw new KeyNotFoundException($"Wallet with ID {request.WalletId} not found.");

            // Update wallet balance
            var newBalance = wallet.Balance + request.Amount;
            await _walletRepository.UpdateBalanceAsync(wallet.Id, newBalance);

            // Record the transaction
            var transaction = new Transaction
            {
                ToWalletId = wallet.Id,
                PaymentMethodId = request.PaymentMethodId,
                Amount = request.Amount,
                Type = TransactionType.TopUp,
                CreatedAt = DateTime.UtcNow,
                Status = TransactionStatus.Pending
            };
            await _transactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new WalletResponseDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = newBalance,
                CreatedAt = wallet.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}