using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionProcessingApp.Models;

namespace TransactionProcessingApp.Services
{
    // This interface defines the contract for a service that provides transaction-related operations.
    public interface IInteractiveService
    {
        Task StartInteractionAsync();
    }
    public interface ITransactionService
    {
        // Gets the total amount of all Credit transactions asynchronously.
        Task<decimal> GetTotalCreditAmountAsync();

        // Gets the total amount of all Debit transactions asynchronously.
        Task<decimal> GetTotalDebitAmountAsync();

        // Gets the date and time of the transaction with the highest amount asynchronously.
        Task<DateTime?> GetTransactionWithHighestAmountDateTimeAsync();

        // Calculates the average transaction amount per day asynchronously.
        Task<decimal> GetAverageAmountPerDayAsync();

        // Gets the top 5 dates with the highest total transaction amounts asynchronously.
        Task<List<DateTime>> GetTop5DatesWithHighestTotalAmountAsync();

    }
}
