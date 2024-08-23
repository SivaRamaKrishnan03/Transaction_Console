using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TransactionProcessingApp.Services;
using TransactionProcessingApp.Repository;

namespace TransactionProcessingApp.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly ITransactionService _transactionService;
         private readonly IInteractiveService _interactiveService;
        private readonly Mock<ITransactionRepository> _mockRepo;
        private readonly Mock<ILoggingService> _mockLogger;

        public TransactionServiceTests()
        {
            _mockRepo = new Mock<ITransactionRepository>();
            _mockLogger = new Mock<ILoggingService>();
            _transactionService = new TransactionService(_mockRepo.Object, _mockLogger.Object);
             _interactiveService = new TransactionService(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetTotalCreditAmountAsync_ReturnsCorrectAmount()
        {
            // Arrange
            var expectedAmount = 1000m;
            _mockRepo.Setup(repo => repo.TotalCreditAmountAsync()).ReturnsAsync(expectedAmount);

            // Act
            var result = await _transactionService.GetTotalCreditAmountAsync();

            // Assert
            Assert.Equal(expectedAmount, result);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("Getting total credit amount."))), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains($"Total credit amount retrieved: {expectedAmount}"))), Times.Once);
        }

        [Fact]
        public async Task GetTotalCreditAmountAsync_ThrowsException_LogsError()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockRepo.Setup(repo => repo.TotalCreditAmountAsync()).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _transactionService.GetTotalCreditAmountAsync());
            Assert.Equal("Test exception", ex.Message);
            _mockLogger.Verify(logger => logger.LogError(exception, "Error retrieving total credit amount."), Times.Once);
        }

        [Fact]
        public async Task GetTotalDebitAmountAsync_ReturnsCorrectAmount()
        {
            // Arrange
            var expectedAmount = 500m;
            _mockRepo.Setup(repo => repo.TotalDebitAmountAsync()).ReturnsAsync(expectedAmount);

            // Act
            var result = await _transactionService.GetTotalDebitAmountAsync();

            // Assert
            Assert.Equal(expectedAmount, result);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("Getting total debit amount."))), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains($"Total debit amount retrieved: {expectedAmount}"))), Times.Once);
        }

        [Fact]
        public async Task GetTotalDebitAmountAsync_ThrowsException_LogsError()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockRepo.Setup(repo => repo.TotalDebitAmountAsync()).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _transactionService.GetTotalDebitAmountAsync());
            Assert.Equal("Test exception", ex.Message);
            _mockLogger.Verify(logger => logger.LogError(exception, "Error retrieving total debit amount."), Times.Once);
        }

        [Fact]
        public async Task GetTransactionWithHighestAmountDateTimeAsync_ReturnsCorrectDateTime()
        {
            // Arrange
            var expectedDate = new DateTime(2024, 8, 12);
            _mockRepo.Setup(repo => repo.TransactionWithHighestAmountDateTimeAsync()).ReturnsAsync(expectedDate);

            // Act
            var result = await _transactionService.GetTransactionWithHighestAmountDateTimeAsync();

            // Assert
            Assert.Equal(expectedDate, result);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("Getting transaction with the highest amount date/time."))), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains($"Transaction with highest amount date/time retrieved: {expectedDate}"))), Times.Once);
        }

        [Fact]
        public async Task GetTransactionWithHighestAmountDateTimeAsync_ThrowsException_LogsError()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockRepo.Setup(repo => repo.TransactionWithHighestAmountDateTimeAsync()).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _transactionService.GetTransactionWithHighestAmountDateTimeAsync());
            Assert.Equal("Test exception", ex.Message);
            _mockLogger.Verify(logger => logger.LogError(exception, "Error retrieving transaction with the highest amount date/time."), Times.Once);
        }

        [Fact]
        public async Task GetAverageAmountPerDayAsync_ReturnsCorrectAverage()
        {
            // Arrange
            var expectedAverage = 250m;
            _mockRepo.Setup(repo => repo.AverageTransactionAmountPerDayAsync()).ReturnsAsync(expectedAverage);

            // Act
            var result = await _transactionService.GetAverageAmountPerDayAsync();

            // Assert
            Assert.Equal(expectedAverage, result);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("Calculating average amount per day."))), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains($"Average amount per day calculated: {expectedAverage}"))), Times.Once);
        }

        [Fact]
        public async Task GetAverageAmountPerDayAsync_ThrowsException_LogsError()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockRepo.Setup(repo => repo.AverageTransactionAmountPerDayAsync()).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _transactionService.GetAverageAmountPerDayAsync());
            Assert.Equal("Test exception", ex.Message);
            _mockLogger.Verify(logger => logger.LogError(exception, "Error calculating average amount per day."), Times.Once);
        }

        [Fact]
        public async Task GetTop5DatesWithHighestTotalAmountAsync_ReturnsCorrectDates()
        {
            // Arrange
            var expectedDates = new List<DateTime>
            {
                new DateTime(2024, 8, 12),
                new DateTime(2024, 8, 11),
                new DateTime(2024, 8, 10),
                new DateTime(2024, 8, 9),
                new DateTime(2024, 8, 8)
            };
            _mockRepo.Setup(repo => repo.Top5DatesWithHighestTransactionAmountAsync()).ReturnsAsync(expectedDates);

            // Act
            var result = await _transactionService.GetTop5DatesWithHighestTotalAmountAsync();

            // Assert
            Assert.Equal(expectedDates, result);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("Getting top 5 dates with the highest total amount."))), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("Top 5 dates with highest total amount retrieved"))), Times.Once);
        }

        [Fact]
        public async Task GetTop5DatesWithHighestTotalAmountAsync_ThrowsException_LogsError()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _mockRepo.Setup(repo => repo.Top5DatesWithHighestTransactionAmountAsync()).ThrowsAsync(exception);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _transactionService.GetTop5DatesWithHighestTotalAmountAsync());
            Assert.Equal("Test exception", ex.Message);
            _mockLogger.Verify(logger => logger.LogError(exception, "Error retrieving top 5 dates with highest total amount."), Times.Once);
        }

        [Fact]
        public async Task StartInteractionAsync_HandlesUserInteractionCorrectly()
        {
            // Arrange
            var inputSequence = new Queue<string>(new[] { "1", "2","3","4","5","6" });
            var expectedCreditAmount = 1000m;

            _mockRepo.Setup(repo => repo.TotalCreditAmountAsync()).ReturnsAsync(expectedCreditAmount);

            // Simulate user input in the console
            Console.SetIn(new System.IO.StringReader(string.Join(Environment.NewLine, inputSequence)));

            // Act
            await _interactiveService.StartInteractionAsync();

            // Assert
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("Starting interaction with the user."))), Times.Once);
            _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("Exiting the application."))), Times.Once);
        }
       
        [Fact]
public async Task StartInteractionAsync_Option5_DisplaysTop5DatesWithHighestTotalAmount()
{
    // Arrange
    var inputSequence = new Queue<string>(new[] { "5", "6" }); // Option 5 followed by exit (6)
    var expectedDates = new List<DateTime>
    {
        new DateTime(2024, 8, 12),
        new DateTime(2024, 8, 11),
        new DateTime(2024, 8, 10),
        new DateTime(2024, 8, 9),
        new DateTime(2024, 8, 8)
    };

    // Simulate user input in the console
    Console.SetIn(new System.IO.StringReader(string.Join(Environment.NewLine, inputSequence)));

    // Capture console output
    using (var consoleOutput = new System.IO.StringWriter())
    {
        Console.SetOut(consoleOutput);

        _mockRepo.Setup(repo => repo.Top5DatesWithHighestTransactionAmountAsync()).ReturnsAsync(expectedDates);

        // Act
        await _interactiveService.StartInteractionAsync();

        // Assert
        var output = consoleOutput.ToString();
        Assert.Contains("Top 5 Dates with Highest Total Amount:", output);
        foreach (var date in expectedDates)
        {
            Assert.Contains($"{date:dd-MM-yyyy}", output); // Ensure each date is printed in the expected format
        }

        _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("Top 5 dates with highest total amount retrieved"))), Times.Once);
    }
}

[Fact]
public async Task StartInteractionAsync_HandlesInvalidMenuSelection()
{
    // Arrange
    var inputSequence = new Queue<string>(new[] { "7", "6" }); // Invalid option followed by exit

    // Simulate user input in the console
    Console.SetIn(new System.IO.StringReader(string.Join(Environment.NewLine, inputSequence)));

    // Capture console output
    using (var consoleOutput = new System.IO.StringWriter())
    {
        Console.SetOut(consoleOutput);

        // Act
        await _interactiveService.StartInteractionAsync();

        // Assert
        var output = consoleOutput.ToString();
        Assert.Contains("Invalid option", output); // Check that the invalid option message is printed

        _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("User selected an invalid menu option."))), Times.Once);
        _mockLogger.Verify(logger => logger.LogInfo(It.Is<string>(msg => msg.Contains("Exiting the application."))), Times.Once);
    }
    
    // Reset the console output back to default
    Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
}

     }
 }
