using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransactionProcessingApp.Models;
using TransactionProcessingApp.Repository;

namespace TransactionProcessingApp.Repository
{
    /// <summary>
    /// Implements the <see cref="ITransactionRepository"/> interface 
    /// and provides methods to perform operations on transactions.
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionContext _context;
        private readonly ILoggingService _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to interact with the database.</param>
        /// <param name="logger">The custom logging service used for logging information and errors.</param>
        public TransactionRepository(TransactionContext context, ILoggingService logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Calculates the total amount of all credit transactions.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total credit amount.</returns>
        public async Task<decimal> TotalCreditAmountAsync()
        {
            _logger.LogInfo("Calculating total credit amount.");
                var totalAmount = await _context.Transactions
                    .Where(t => t.Type == "Credit") 
                    .SumAsync(t => t.Amount);

                _logger.LogInfo($"Total credit amount calculated: {totalAmount}");
                return totalAmount;
        }

        /// <summary>
        /// Calculates the total amount of all debit transactions.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total debit amount.</returns>
        public async Task<decimal> TotalDebitAmountAsync()
        {
            _logger.LogInfo("Calculating total debit amount.");
                var totalAmount = await _context.Transactions
                    .Where(t => t.Type == "Debit") 
                    .SumAsync(t => t.Amount);

                _logger.LogInfo($"Total debit amount calculated: {totalAmount}");
                return totalAmount;
        }

        /// <summary>
        /// Finds the date and time of the transaction with the highest amount.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the date and time of the transaction with the highest amount, or null if no transactions exist.</returns>
        public async Task<DateTime?> TransactionWithHighestAmountDateTimeAsync()
        {
            _logger.LogInfo("Finding transaction with the highest amount.");
                var dateTime = await _context.Transactions
                    .OrderByDescending(t => t.Amount) 
                    .Select(t => t.Date) 
                    .FirstOrDefaultAsync();

                _logger.LogInfo($"Transaction with the highest amount date/time found: {dateTime}");
                return dateTime;
        }

        /// <summary>
        /// Calculates the average transaction amount per day.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the average transaction amount per day.</returns>
        public async Task<decimal> AverageTransactionAmountPerDayAsync()
        {
            _logger.LogInfo("Calculating average transaction amount per day.");
             {
                var dailyAverages = await _context.Transactions
                    .GroupBy(t => t.Date.Date) 
                    .Select(g => g.Average(t => t.Amount)) 
                    .ToListAsync(); 

                decimal result = 0;

                if (dailyAverages.Any())
                {
                    result = Math.Round(dailyAverages.Average(), 2); 
                }

                _logger.LogInfo($"Average transaction amount per day calculated: {result}");
                return result;
           }
        }

        /// <summary>
        /// Finds the top 5 dates with the highest total transaction amounts.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of the top 5 dates with the highest total transaction amounts.</returns>
        public async Task<List<DateTime>> Top5DatesWithHighestTransactionAmountAsync()
        {
            _logger.LogInfo("Finding top 5 dates with the highest total transaction amounts.");
             {
                var topDates = await _context.Transactions
                    .GroupBy(t => t.Date.Date)
                    .OrderByDescending(g => g.Sum(t => t.Amount)) 
                    .Take(5)
                    .Select(g => g.Key) 
                    .ToListAsync();

                _logger.LogInfo($"Top 5 dates with highest total transaction amounts found: {string.Join(", ", topDates)}");
                return topDates;
            }
        }
    }
}

