using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using TransactionProcessingApp.Models;
using TransactionProcessingApp.Repository;

namespace TransactionProcessingApp.Tests.Repositories
{
    public class TransactionRepositoryTests
    {
        private readonly TransactionContext _context;
        private readonly TransactionRepository _transactionRepository;
        private readonly Mock<ILoggingService> _mockLogger;

        public TransactionRepositoryTests()
        {
            // Setup the in-memory database context
            var options = new DbContextOptionsBuilder<TransactionContext>()
                .UseInMemoryDatabase(databaseName: "TransactionTestDb")
                .Options;

            _context = new TransactionContext(options);

            // Seed data
            SeedData();

            // Create a mock logging service
            _mockLogger = new Mock<ILoggingService>();

            // Initialize the repository with the in-memory context and mock logger
            _transactionRepository = new TransactionRepository(_context, _mockLogger.Object);
        }

        private void SeedData()
        {
            if (!_context.Transactions.Any())
            {
                var transactions = new List<Transaction>
                {
                    new Transaction { Id = 1, Date = new DateTime(2024, 8, 12), Type = "Credit", Amount = 1000m },
                    new Transaction { Id = 2, Date = new DateTime(2024, 8, 11), Type = "Debit", Amount = 500m },
                    new Transaction { Id = 3, Date = new DateTime(2024, 8, 10), Type = "Credit", Amount = 300m },
                    new Transaction { Id = 4, Date = new DateTime(2024, 8, 10), Type = "Credit", Amount = 200m },
                    new Transaction { Id = 5, Date = new DateTime(2024, 8, 9), Type = "Debit", Amount = 150m }
                };

                _context.Transactions.AddRange(transactions);
                _context.SaveChanges();
            }
        }

        [Fact]
        public async Task TotalCreditAmountAsync_ReturnsCorrectAmount()
        {
            // Arrange
            var expectedAmount = 1500m;

            // Act
            var result = await _transactionRepository.TotalCreditAmountAsync();

            // Assert
            Assert.Equal(expectedAmount, result);

            // Verify logging
            _mockLogger.Verify(logger => logger.LogInfo("Calculating total credit amount."), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo($"Total credit amount calculated: {expectedAmount}"), Times.Once);
        }

        [Fact]
        public async Task TotalDebitAmountAsync_ReturnsCorrectAmount()
        {
            // Arrange
            var expectedAmount = 650m;

            // Act
            var result = await _transactionRepository.TotalDebitAmountAsync();

            // Assert
            Assert.Equal(expectedAmount, result);

            // Verify logging
            _mockLogger.Verify(logger => logger.LogInfo("Calculating total debit amount."), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo($"Total debit amount calculated: {expectedAmount}"), Times.Once);
        }

        [Fact]
        public async Task TransactionWithHighestAmountDateTimeAsync_ReturnsCorrectDateTime()
        {
            // Arrange
            var expectedDate = new DateTime(2024, 8, 12);

            // Act
            var result = await _transactionRepository.TransactionWithHighestAmountDateTimeAsync();

            // Assert
            Assert.Equal(expectedDate, result);

            // Verify logging
            _mockLogger.Verify(logger => logger.LogInfo("Finding transaction with the highest amount."), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo($"Transaction with the highest amount date/time found: {expectedDate}"), Times.Once);
        }

        [Fact]
        public async Task AverageTransactionAmountPerDayAsync_ReturnsCorrectAverage()
        {
            // Arrange
            var expectedAverage = 475m;

            // Act
            var result = await _transactionRepository.AverageTransactionAmountPerDayAsync();

            // Assert
            Assert.Equal(expectedAverage, result);

            // Verify logging
            _mockLogger.Verify(logger => logger.LogInfo("Calculating average transaction amount per day."), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo($"Average transaction amount per day calculated: {expectedAverage}"), Times.Once);
        }


        [Fact]
        public async Task Top5DatesWithHighestTransactionAmountAsync_ReturnsCorrectDates()
        {
            // Arrange
            var expectedDates = new List<DateTime>
            {
                new DateTime(2024, 8, 12),
                new DateTime(2024, 8, 11),
                new DateTime(2024, 8, 10),
                new DateTime(2024, 8, 9)
            };

            // Act
            var result = await _transactionRepository.Top5DatesWithHighestTransactionAmountAsync();

            // Assert
            Assert.Equal(expectedDates, result);

            // Verify logging
            _mockLogger.Verify(logger => logger.LogInfo("Finding top 5 dates with the highest total transaction amounts."), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo($"Top 5 dates with highest total transaction amounts found: {string.Join(", ", expectedDates)}"), Times.Once);
        }
    }
}
