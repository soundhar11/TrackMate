using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace TrackMate.Database
{
    public class Transaction
    {
        private readonly string _connectionString;

        public Transaction(string databasePath)
        {
            _connectionString = $"Data Source={databasePath};Version=3;";
            CreateTable();
        }

        // Create tables if they don't exist
        private void CreateTable()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Create Transactions table
                string createTransactionsTable = @"
                CREATE TABLE IF NOT EXISTS Transactions (
                    TransactionId INTEGER PRIMARY KEY AUTOINCREMENT,
                    MachineryId INTEGER NOT NULL,
                    UserId INTEGER NOT NULL,
                    StartDate TEXT NOT NULL,
                    EndDate TEXT,
                    FOREIGN KEY (MachineryId) REFERENCES Details(Id),
                    FOREIGN KEY (UserId) REFERENCES Users(Id)
                );";

                using (var command = new SQLiteCommand(createTransactionsTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Insert a new transaction and update the previous one
        public void InsertTransaction(int machineryId, int userId, string startDate, string? endDate)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Transactions (MachineryId, UserId, StartDate, EndDate) VALUES (@MachineryId, @UserId, @StartDate, @EndDate)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MachineryId", machineryId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", (object?)endDate ?? DBNull.Value); // Handle null
                    command.ExecuteNonQuery();
                }
            }
        }
        public int GetTransactionIdByProductIdUserId(int productId, int userId)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT TransactionId FROM Transactions WHERE MachineryId = @MachineryId AND UserId = @UserId AND EndDate IS NULL";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MachineryId", productId);
                        command.Parameters.AddWithValue("@UserId", userId);

                        var result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int transactionId))
                        {
                            return transactionId;
                        }
                    }
                }
                return -1; // Transaction not found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching TransactionId: {ex.Message}");
                return -1;
            }
        }

        public void UpdateEndDate(int transactionID, string endDate)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Transactions SET EndDate = @EndDate WHERE TransactionId = @TransactionId AND EndDate IS NULL";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@TransactionId", transactionID);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("No rows updated. Either UserId not found or EndDate is already set.");
                    }
                }
            }
        }

        public void UpdateStartDate(int transactionID, string startDate)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Transactions SET StartDate = @StartDate WHERE TransactionId = @TransactionId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@TransactionId", transactionID);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("No rows updated. Either UserId not found or EndDate is already set.");
                    }
                }
            }
        }



        // Retrieve all transactions
        public List<TransactionModel> GetTransactionsByMachineryId(int machineryId)
        {
            var transactions = new List<TransactionModel>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = @"
            SELECT 
                t.TransactionId,
                d.Name AS MachineryName,
                u.Name AS UserName,
                t.StartDate,
                t.EndDate
            FROM 
                Transactions t
            JOIN 
                Details d ON t.MachineryId = d.Id
            JOIN 
                Users u ON t.UserId = u.Id
            WHERE 
                t.MachineryId = @MachineryId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    // Add the filter parameter
                    command.Parameters.AddWithValue("@MachineryId", machineryId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactions.Add(new TransactionModel
                            {
                                TransactionId = reader.GetInt32(0),
                                MachineryName = reader.GetString(1),
                                UserName = reader.GetString(2),
                                StartDate = reader.GetString(3),
                                EndDate = reader.IsDBNull(4) ? null : reader.GetString(4)
                            });
                        }
                    }
                }
            }
            return transactions;
        }
    }


        // Transaction model
        public class TransactionModel
    {
        public int TransactionId { get; set; }
        public string MachineryName { get; set; }
        public string UserName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
