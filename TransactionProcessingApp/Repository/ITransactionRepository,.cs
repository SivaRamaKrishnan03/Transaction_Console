using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionProcessingApp.Models;

namespace TransactionProcessingApp.Repository
{
    // This interface defines the contract for a repository that handles transaction data.
    public interface ITransactionRepository
    {
        // Gets the total amount of all Credit transactions asynchronously.
        Task<decimal> TotalCreditAmountAsync();

        // Gets the total amount of all Debit transactions asynchronously.
        Task<decimal> TotalDebitAmountAsync();

        // Gets the date and time of the transaction with the highest amount asynchronously.
        Task<DateTime?> TransactionWithHighestAmountDateTimeAsync();

        // Calculates the average transaction amount per day asynchronously.
        Task<decimal> AverageTransactionAmountPerDayAsync();

        // Gets the top 5 dates with the highest total transaction amounts asynchronously.
        Task<List<DateTime>> Top5DatesWithHighestTransactionAmountAsync();
    }
}
