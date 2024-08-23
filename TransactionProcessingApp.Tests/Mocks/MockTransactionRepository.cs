using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionProcessingApp.Models;
using TransactionProcessingApp.Repository;

namespace TransactionProcessingApp.Tests.Mocks
{
    public class MockTransactionRepository : ITransactionRepository
    {
        private readonly List<Transaction> _transactions;

        public MockTransactionRepository(List<Transaction> transactions)
        {
            _transactions = transactions;
        }

        public Task<decimal> TotalCreditAmountAsync()
        {
            return Task.FromResult(_transactions.Where(t => t.Type == "Credit").Sum(t => t.Amount));
        }

        public Task<decimal> TotalDebitAmountAsync()
        {
            return Task.FromResult(_transactions.Where(t => t.Type == "Debit").Sum(t => t.Amount));
        }

        public Task<DateTime?> TransactionWithHighestAmountDateTimeAsync()
        {
            var maxTransaction = _transactions.OrderByDescending(t => t.Amount).FirstOrDefault();
            return Task.FromResult(maxTransaction?.Date);
        }

        public Task<decimal> AverageTransactionAmountPerDayAsync()
        {
            var dailyAverages = _transactions
                .GroupBy(t => t.Date.Date)
                .Select(g => g.Average(t => t.Amount))
                .ToList();

            var average = dailyAverages.Any() ? dailyAverages.Average() : 0;
            return Task.FromResult(Math.Round(average, 2));
        }

        public Task<List<DateTime>> Top5DatesWithHighestTransactionAmountAsync()
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
