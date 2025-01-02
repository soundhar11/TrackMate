using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TrackMate.Database;

namespace TrackMate.Pages
{
    public partial class ChangeOwnershipPage : Page
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly Users _Users;
        private readonly Transaction _transaction;
        private readonly int _itemId;
        private readonly string _userName;
        private readonly string _product;
        private readonly string _date;

        // Constructor to accept itemId
        public ChangeOwnershipPage(int itemId, string product, string userName, string date)
        {
            InitializeComponent();
            string databasePath = "MaterialsTracker.db"; // Adjust path as needed
            _databaseHelper = new DatabaseHelper(databasePath);
            _Users = new Users(databasePath);
            _transaction = new Transaction(databasePath);

            _itemId = itemId;  // Assign passed itemId
            _product = product;
            _userName = userName;
            _date = date;
            LoadProductDetails();
            LoadUsernames();
        }

        private void LoadProductDetails()
        {
            if (_product != null)
            {
                // Set the product name (not editable)
                ProductNameTextBlock.Text = _product;
            }
            else
            {
                MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Filter usernames as the user types
        private void UsernameComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string enteredText = UsernameComboBox.Text.ToLower();

            // Fetch matching usernames from the database
            List<string> filteredUsernames = _Users.GetUsernamesByPrefix(enteredText);

            if (filteredUsernames.Count > 0)
            {
                // Update the ComboBox with matching usernames
                UsernameComboBox.ItemsSource = filteredUsernames;
                UsernameComboBox.IsDropDownOpen = true; // Show dropdown with matches
            }
            else
            {
                // Clear the ComboBox if no matches are found
                UsernameComboBox.ItemsSource = null;
                UsernameComboBox.IsDropDownOpen = false; // Close dropdown
            }
        }

        // Load all usernames into the ComboBox when the page loads
        private void LoadUsernames()
        {
            try
            {
                List<string> usernames = _Users.GetAllUsernames();
                UsernameComboBox.ItemsSource = usernames;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading usernames: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Ensure only a valid date is selected
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = DatePicker.SelectedDate.Value;

                // Keep the month and day while updating the year
                if (selectedDate.Year != DatePicker.SelectedDate.Value.Year)
                {
                    DateTime newDate = new DateTime(selectedDate.Year, DatePicker.SelectedDate.Value.Month, DatePicker.SelectedDate.Value.Day);
                    DatePicker.SelectedDate = newDate;
                }
            }
        }

        // Update the ownership details in the database
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string selectedUsername = UsernameComboBox.Text;
            DateTime? selectedDate = DatePicker.SelectedDate;
            int userId = _Users.GetUserIdByName(_userName);

            try
            {
                if (string.IsNullOrWhiteSpace(selectedUsername))
                {
                    MessageBox.Show("Please select a valid username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // Prevent further execution if the username is null or empty
                }
                //  Validate if the date is selected
                if (!selectedDate.HasValue)
                {
                    MessageBox.Show("Please select a date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // Prevent further execution if the date is empty
                }

                //  Check if the selected date is not in the past
                // Step 3: Check if the selected date is not earlier than the provided date (_date)
                if (DateTime.TryParse(_date, out DateTime parsedDate))
                {
                    if (selectedDate.Value.Date < parsedDate.Date)
                    {
                        MessageBox.Show("The selected date cannot be earlier than the specified date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; // Prevent further execution if the date is earlier than _date
                    }
                }
                else
                {
                    MessageBox.Show("Invalid reference date provided.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // Prevent further execution if _date is invalid
                }


                //  Get the current owner of the item
                string currentOwner = _databaseHelper.GetCurrentOwner(_itemId);

                // : Check if the selected user is the same as the current owner
                if (selectedUsername == currentOwner)
                {
                    MessageBox.Show("The selected user is already the owner of this item.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // Prevent further execution if the user is the same
                }

                // Step 5: Proceed with the update if all validations pass
                _databaseHelper.UpdateOwnership(_itemId, selectedUsername, selectedDate.Value.ToString("yyyy-MM-dd"));

                int transactionID = _transaction.GetTransactionIdByProductIdUserId(_itemId, userId);
                int UpdatedUserId = _Users.GetUserIdByName(selectedUsername);
                userId = UpdatedUserId;

                _transaction.UpdateEndDate(transactionID, selectedDate.Value.ToString("yyyy-MM-dd"));
                _transaction.InsertTransaction(_itemId, UpdatedUserId, selectedDate.Value.ToString("yyyy-MM-dd"), null);

                MessageBox.Show("Ownership updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating ownership: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Navigate back to the previous page
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EquipmentDetails());
        }
    }
}
