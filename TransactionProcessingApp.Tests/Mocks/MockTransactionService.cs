using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionProcessingApp.Models;
using TransactionProcessingApp.Repository;
using TransactionProcessingApp.Services;

namespace TransactionProcessingApp.Tests.Mocks
{
    public class MockTransactionService : ITransactionService
    {
        private readonly List<Transaction> _transactions;

        public MockTransactionService(List<Transaction> transactions)
        {
            _transactions = transactions;
        }

        public Task<decimal> GetTotalCreditAmountAsync()
        {
            var totalCredit = _transactions.Where(t => t.Type == "Credit").Sum(t => t.Amount);
            return Task.FromResult(totalCredit);
        }

        public Task<decimal> GetTotalDebitAmountAsync()
        {
            var totalDebit = _transactions.Where(t => t.Type == "Debit").Sum(t => t.Amount);
            return Task.FromResult(totalDebit);
        }

        public Task<DateTime?> GetTransactionWithHighestAmountDateTimeAsync()
        {
            var maxTransaction = _transactions.OrderByDescending(t => t.Amount).FirstOrDefault();
            return Task.FromResult(maxTransaction?.Date);
        }

        public Task<decimal> GetAverageAmountPerDayAsync()
        {
            var dailyAverages = _transactions
                .GroupBy(t => t.Date.Date)
                .Select(g => g.Average(t => t.Amount))
                .ToList();

            var average = dailyAverages.Any() ? dailyAverages.Average() : 0;
            return Task.FromResult(Math.Round(average, 2));
        }

        public Task<List<DateTime>> GetTop5DatesWithHighestTotalAmountAsync()
        {
            var topDates = _transactions
                .GroupBy(t => t.Date.Date)
                .OrderByDescending(g => g.Sum(t => t.Amount))
                .Take(5)
                .Select(g => g.Key)
                .ToList();

            return Task.FromResult(topDates);
        }
    }
}

