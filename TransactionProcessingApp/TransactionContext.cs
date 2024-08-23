using Microsoft.EntityFrameworkCore;
using TransactionProcessingApp.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Diagnostics.CodeAnalysis;

namespace TransactionProcessingApp
{
   
    [ExcludeFromCodeCoverage]
    public class TransactionContext : DbContext
    {
        /// <summary>
        /// Gets or sets the collection of transactions in the database.
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the <see cref="DbContext"/>.</param>
        public TransactionContext(DbContextOptions<TransactionContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configures the database connection to use PostgreSQL if it has not been configured.
        /// </summary>
        /// <param name="optionsBuilder">The builder used to configure the <see cref="DbContext"/>.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Sets up PostgreSQL as the database provider using the provided connection string.
                optionsBuilder.UseNpgsql("Host=192.168.11.136;Database=training_batch_4;Username=batch4;Password=$E4g059vQz5");
            }
        }

        /// <summary>
        /// Configures the model for the database, including table and column mappings.
        /// </summary>
        /// <param name="modelBuilder">The builder used to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configures the Date property of the Transaction entity to use the PostgreSQL timestamp type.
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Date)
                .HasColumnType("timestamp");
        }
    }
}
