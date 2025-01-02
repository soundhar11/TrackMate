using System;
using System.Windows;
using System.Windows.Controls;
using TrackMate.Database;  // Reference the DbHelper class

namespace TrackMate.Pages
{
    public partial class AddUserPage : Page
    {
        private readonly UserDetails db;

        // Constructor that accepts the database path and initializes DbHelper
        public AddUserPage()
        {
            InitializeComponent();
            string databasePath = "MaterialsTracker.db";
            db = new UserDetails(databasePath);  // Initialize DbHelper with database path
            db.CreateTableIfNotExists();  // Ensure the table exists
        }

        // Button click event to add a new user
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;

            // Check if the username is not empty
            if (!string.IsNullOrEmpty(username))
            {
                // Call the method to add the user to the database
                bool success = db.AddUser(username);

                if (success)
                {
                    MessageBox.Show("User added successfully!");
                    UsernameTextBox.Clear(); // Clear the text box after adding the user
                }
                else
                {
                    MessageBox.Show("Failed to add user.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a username.");
            }
        }

        // Back button click event
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the previous page
            NavigationService.GoBack();
        }
    }
}
