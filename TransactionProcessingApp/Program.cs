using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TransactionProcessingApp.Models;
using TransactionProcessingApp.Repository;
using TransactionProcessingApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessingApp
{
    [ExcludeFromCodeCoverage]
   
    class Program
    {
        /// <summary>
/// The main entry point for the application.
/// This method sets up the host, initializes services, seeds data, and starts the interactive service.
/// </summary>
/// <param name="args">An array of command-line arguments passed to the application.</param>
/// <returns>A Task representing the asynchronous operation of the application.</returns>
static async Task Main(string[] args)
{
    // Build the host for dependency injection and configuration
    var host = CreateHostBuilder(args).Build();

    // Create a new scope for the application services
    using (var scope = host.Services.CreateScope())
    {
        // Get the required services from the service provider
        var services = scope.ServiceProvider;
        var loggingService = services.GetRequiredService<ILoggingService>();
        var context = services.GetRequiredService<TransactionContext>();
        var interactiveService = services.GetRequiredService<IInteractiveService>();

        try
        {
            // Log the start of the application
            loggingService.LogInfo("Application Starting...");

            // Seed initial data into the database
            SeedData(context);

            // Start the interactive service for user interactions
            await interactiveService.StartInteractionAsync();
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur
            loggingService.LogError(ex, "Application encountered an error.");
            throw;
        }
        finally
        {
            // Ensure the logging system is properly shut down
            LogManager.Shutdown();
        }
    }
}

       /// <summary>
/// Creates and configures an <see cref="IHostBuilder"/> for the application.
/// Sets up the default configurations, service registrations, and logging providers.
/// </summary>
/// <param name="args">An array of command-line arguments passed to the application.</param>
/// <returns>An <see cref="IHostBuilder"/> configured with services and logging providers.</returns>
private static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            try
            {
                // Configures the DbContext to use PostgreSQL with the specified connection string
                services.AddDbContext<TransactionContext>(options =>
                    options.UseNpgsql("Host=192.168.11.136;Database=training_batch_4;Username=batch4;Password=$E4g059vQz5"));

                // Registers the TransactionRepository as the implementation of ITransactionRepository
                services.AddScoped<ITransactionRepository, TransactionRepository>();

                // Registers the TransactionService as the implementation of ITransactionService
                services.AddScoped<ITransactionService, TransactionService>();

                // Registers the TransactionService as the implementation of IInteractiveService
                services.AddScoped<IInteractiveService, TransactionService>();

                // Registers the LoggingService as the implementation of ILoggingService
                services.AddScoped<ILoggingService, LoggingService>();
            }
            catch (Npgsql.PostgresException ex)
            {
                // Logs any PostgreSQL-specific exceptions that occur during service configuration
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Error(ex, $"A Postgres error occurred: {ex.MessageText}. SQL State: {ex.SqlState}");
                throw;
            }
            catch (Exception ex)
            {
                // Logs any general exceptions that occur during service configuration
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Error(ex, "An unexpected error occurred while configuring services.");
                throw;
            }
        })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();

            logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);

            logging.AddNLog();
        });

        /// <summary>
        /// Seeds the database with initial data if the database is empty.
        /// </summary>
        /// <param name="context">The database context used to interact with the database.</param>
        private static void SeedData(TransactionContext context)
        {
            if (!context.Transactions.Any())
            {
                var jsonData = File.ReadAllText("transaction.json");
                var transactions = JsonConvert.DeserializeObject<List<Transaction>>(jsonData);

                if (transactions != null)
                {
                    context.Transactions.AddRange(transactions);
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("No data was found in the JSON file.");
                }
            }
        }
    }
}






















