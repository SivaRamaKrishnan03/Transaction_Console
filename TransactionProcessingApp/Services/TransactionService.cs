using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionProcessingApp.Models;
using TransactionProcessingApp.Repository;

namespace TransactionProcessingApp.Services
{
    /// <summary>
    /// Provides services for handling transactions, including calculations and interactions with users.
    /// Implements <see cref="IInteractiveService"/> and <see cref="ITransactionService"/>.
    /// </summary>
    public class TransactionService : IInteractiveService, ITransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly ILoggingService _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class.
        /// </summary>
        /// <param name="repository">The transaction repository used for data access.</param>
        /// <param name="logger">The custom logging service used for logging information and errors.</param>
        public TransactionService(ITransactionRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Gets the total amount of all credit transactions asynchronously.
        /// </summary>
        /// <returns>The total credit amount.</returns>
        public async Task<decimal> GetTotalCreditAmountAsync()
        {
            _logger.LogInfo("Getting total credit amount.");
            try
            {
                var result = await _repository.TotalCreditAmountAsync();
                _logger.LogInfo($"Total credit amount retrieved: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total credit amount.");
                throw;
            }
        }

        /// <summary>
        /// Gets the total amount of all debit transactions asynchronously.
        /// </summary>
        /// <returns>The total debit amount.</returns>
        public async Task<decimal> GetTotalDebitAmountAsync()
        {
            _logger.LogInfo("Getting total debit amount.");
            try
            {
                var result = await _repository.TotalDebitAmountAsync();
                _logger.LogInfo($"Total debit amount retrieved: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total debit amount.");
                throw;
            }
        }

        /// <summary>
        /// Gets the date and time of the transaction with the highest amount asynchronously.
        /// </summary>
        /// <returns>The date and time of the transaction with the highest amount, or null if no transactions exist.</returns>
        public async Task<DateTime?> GetTransactionWithHighestAmountDateTimeAsync()
        {
            _logger.LogInfo("Getting transaction with the highest amount date/time.");
            try
            {
                var result = await _repository.TransactionWithHighestAmountDateTimeAsync();
                _logger.LogInfo($"Transaction with highest amount date/time retrieved: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transaction with the highest amount date/time.");
                throw;
            }
        }

        /// <summary>
        /// Calculates the average transaction amount per day asynchronously.
        /// </summary>
        /// <returns>The average transaction amount per day.</returns>
        public async Task<decimal> GetAverageAmountPerDayAsync()
        {
            _logger.LogInfo("Calculating average amount per day.");
            try
            {
                var result = await _repository.AverageTransactionAmountPerDayAsync();
                _logger.LogInfo($"Average amount per day calculated: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating average amount per day.");
                throw;
            }
        }

        /// <summary>
        /// Gets the top 5 dates with the highest total transaction amounts asynchronously.
        /// </summary>
        /// <returns>A list of the top 5 dates with the highest total transaction amounts.</returns>
        public async Task<List<DateTime>> GetTop5DatesWithHighestTotalAmountAsync()
        {
            _logger.LogInfo("Getting top 5 dates with the highest total amount.");
            try
            {
                var result = await _repository.Top5DatesWithHighestTransactionAmountAsync();
                _logger.LogInfo($"Top 5 dates with highest total amount retrieved: {string.Join(", ", result)}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top 5 dates with highest total amount.");
                throw;
            }
        }

        /// <summary>
        /// Starts an interactive console session with the user, allowing them to choose various options for viewing transaction data.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task StartInteractionAsync()
        {
            _logger.LogInfo("Starting interaction with the user.");
            while (true)
            {
                // Displays the menu options to the user.
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Total Credit Amount");
                Console.WriteLine("2. Total Debit Amount");
                Console.WriteLine("3. Transaction with Highest Amount DateTime");
                Console.WriteLine("4. Average Amount Per Day");
                Console.WriteLine("5. Top 5 Dates with Highest Total Amount");
                Console.WriteLine("6. Exit");

                var input = Console.ReadLine();
             
                // Handles the user's menu selection.
                try
                {
                    switch (input)
                    {
                        case "1":
                            Console.WriteLine($"Total Credit Amount: {await GetTotalCreditAmountAsync()}");
                            break;
                        case "2":
                            Console.WriteLine($"Total Debit Amount: {await GetTotalDebitAmountAsync()}");
                            break;
                        case "3":
                            Console.WriteLine($"Transaction with Highest Amount DateTime: {await GetTransactionWithHighestAmountDateTimeAsync()}");
                            break;
                        case "4":
                            Console.WriteLine($"Average Amount Per Day: {await GetAverageAmountPerDayAsync()}");
                            break;
                        case "5":
                            var topDates = await GetTop5DatesWithHighestTotalAmountAsync();
                            Console.WriteLine("Top 5 Dates with Highest Total Amount:");
                            foreach (var date in topDates)
                            {
                                Console.WriteLine($"{date:dd-MM-yyyy}");
                            }
                            break;
                        case "6":
                            Console.WriteLine("Exiting...");
                            _logger.LogInfo("Exiting the application.");
                            return;
                        default:
                            Console.WriteLine("Invalid option, please try again.");
                            _logger.LogInfo("User selected an invalid menu option.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during user interaction.");
                }
            }
        }
    }
}
