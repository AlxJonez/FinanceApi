using FinanceApi.Infrastructure.Abstraction;
using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly MoneyConverterService _moneyConverterService;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IBudgetRepository budgetRepository,
            MoneyConverterService moneyConverterService)
        {
            _transactionRepository = transactionRepository;
            _budgetRepository = budgetRepository;
            _moneyConverterService = moneyConverterService;
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            var budget = await _budgetRepository.GetByIdAsync(transaction.BudgetId)
                         ?? throw new Exception("Бюджет не найден");

            decimal adjustedAmount = transaction.Amount;

            // Конвертация если валюты не совпадают
            if (transaction.Currency != budget.Currency)
            {
                adjustedAmount = await _moneyConverterService.Convert(
                    transaction.Currency, budget.Currency, transaction.Amount);
            }

            // Изменение баланса в зависимости от типа транзакции
            if (transaction.Type == TransactionType.Expense)
                budget.CurrentBalance -= adjustedAmount;
            else
                budget.CurrentBalance += adjustedAmount;

            transaction.CreatedAt = DateTime.UtcNow;
            transaction.Date = transaction.Date == default ? DateTime.UtcNow : transaction.Date;

            await _transactionRepository.CreateAsync(transaction);
            await _budgetRepository.UpdateAsync(budget.Id, budget);

            return transaction;
        }

        public async Task<Transaction?> UpdateAsync(Transaction updated)
        {
            var existing = await _transactionRepository.GetByIdAsync(updated.Id)
                            ?? throw new Exception("Транзакция не найдена");

            if (existing.BudgetId != updated.BudgetId || existing.Amount != updated.Amount ||
                existing.Currency != updated.Currency || existing.Type != updated.Type)
            {
                // Откат старой транзакции
                var oldBudget = await _budgetRepository.GetByIdAsync(existing.BudgetId)
                               ?? throw new Exception("Бюджет не найден");
                decimal oldAmount = existing.Amount;

                if (existing.Currency != oldBudget.Currency)
                    oldAmount = await _moneyConverterService.Convert(existing.Currency, oldBudget.Currency, existing.Amount);

                if (existing.Type == TransactionType.Expense)
                    oldBudget.CurrentBalance += oldAmount;
                else
                    oldBudget.CurrentBalance -= oldAmount;

                await _budgetRepository.UpdateAsync(oldBudget.Id, oldBudget);
            }

            // Применение новой транзакции
            await _transactionRepository.UpdateAsync(updated.Id, updated);

            return updated;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tx = await _transactionRepository.GetByIdAsync(id)
                     ?? throw new Exception("Транзакция не найдена");

            var budget = await _budgetRepository.GetByIdAsync(tx.BudgetId)
                         ?? throw new Exception("Бюджет не найден");

            decimal amount = tx.Amount;
            if (tx.Currency != budget.Currency)
                amount = await _moneyConverterService.Convert(tx.Currency, budget.Currency, tx.Amount);

            if (tx.Type == TransactionType.Expense)
                budget.CurrentBalance += amount;
            else
                budget.CurrentBalance -= amount;

            await _transactionRepository.DeleteAsync(id);
            await _budgetRepository.UpdateAsync(budget.Id, budget);
            return true;
        }

        public async Task<List<Transaction>> GetAllByPeriodAsync(Guid userId, DateTime from, DateTime to, int skip, int take)
        {
            var all = await _transactionRepository.GetAllAsync();
            return all
                .Where(x => x.UserId == userId && x.Date >= from && x.Date <= to)
                .OrderByDescending(x => x.Date)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public async Task<List<Transaction>> GetLast(Guid userId, int skip, int take)
        {
            return await _transactionRepository.GetLastTransactions(userId, skip, take);

        }



        //добавить поступление в бюджет

        //переместить средства из одного бюджета в другой (в подпапку или наоборот)

        //зарегистрировать расход в категорию

        //получить список последних ХХ расходов

        //изменить расход (сумма, комментарий, категория)

        //получение расходов за период

        //получение суммы расходов за период (только число)

    }
}
