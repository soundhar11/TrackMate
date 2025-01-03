using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TrackMate.Database;
using static TrackMate.Database.DatabaseHelper;

namespace TrackMate.Pages
{
    public partial class EquipmentDetails : Page
    {
        private DatabaseHelper _databaseHelper;
        private string CurrentFilter = string.Empty; // Tracks the active filter (Name/Username)

        public EquipmentDetails()
        {
            InitializeComponent();
            _databaseHelper = new DatabaseHelper("MaterialsTracker.db"); // Provide the correct path
            this.Loaded += OnPageLoaded;
            LoadData();
        }
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            // Subscribe to the Navigated event when the page is fully loaded
            this.NavigationService.Navigated += OnNavigated;

            // Now call LoadData() to refresh data
            LoadData();
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            // Reload data when navigating to this page
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                // Retrieve product details by name
                List<Product> products = _databaseHelper.GetProductDetails();

                if (products.Any())
                {
                    // Display data in the DataGrid
                    DetailsDataGrid.ItemsSource = products;
                }
                //else
                //{
                //    MessageBox.Show("No product found.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Load all names into the ComboBox
        

        // Retrieve and display details for the selected name
        private void DisplayDetailsForSelectedName(string name)
        {
            try
            {
                // Retrieve product details by name
                List<Product> products = _databaseHelper.GetProductDetailsByName(name);

                if (products.Any())
                {
                    // Display data in the DataGrid
                    DetailsDataGrid.ItemsSource = products;
                }
                //else
                //{
                //    MessageBox.Show("No product found for the selected name.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OwnerShip_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected row in the DataGrid
            var selectedProduct = DetailsDataGrid.SelectedItem as Product; // Assuming the binding is to a 'Product' class

            if (selectedProduct != null)
            {
                // Navigate to ChangeOwnershipPage, passing the product ID
                NavigationService.Navigate(new ChangeOwnershipPage(selectedProduct.Id,selectedProduct.Name,selectedProduct.Username,selectedProduct.Date));
            }
            else
            {
                MessageBox.Show("Please select a product first.");
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected product from the DataGrid
            var selectedProduct = DetailsDataGrid.SelectedItem as Product;

            if (selectedProduct != null)
            {
                // Pass the productId to the EditPage
                NavigationService.Navigate(new EditPage(selectedProduct.Id,selectedProduct.Name));
            }
            else
            {
                MessageBox.Show("Please select a product first.");
            }
        }


        // Display the image for the selected product
        private void DisplayImageForSelectedProduct(Product product)
        {
            try
            {
                if (product.Photo != null && product.Photo.Length > 0)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(product.Photo))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }

                    // Display the image in the WPF Image control
                   // DisplayImage.Source = bitmapImage;
                }
                else
                {
                   // DisplayImage.Source = null; // Clear the image if no data is found
                    MessageBox.Show("No image found for the selected product.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private ListSortDirection _sortDirection = ListSortDirection.Ascending; // Default sort direction

        private void FilterImage_Click(object sender, RoutedEventArgs e)
        {
            // Create a ContextMenu dynamically
            ContextMenu filterMenu = new ContextMenu();

            // Add "Name" filter option
            MenuItem nameFilter = new MenuItem() { Header = "Name" };
            nameFilter.Click += (s, args) => ShowNameFilter(); // Show Name Filter UI

            // Add "Username" filter option
            MenuItem usernameFilter = new MenuItem() { Header = "Username" };
            usernameFilter.Click += (s, args) => ShowUsernameFilter(); // Show Username Filter UI

            // Add "Date" filter option
            MenuItem dateFilter = new MenuItem() { Header = "Date" };
            dateFilter.Click += (s, args) => ShowDatePicker(); // Show Date Picker for filtering by Date

            // Add options to the ContextMenu
            filterMenu.Items.Add(nameFilter);
            filterMenu.Items.Add(usernameFilter);
            filterMenu.Items.Add(dateFilter);

            // Open the ContextMenu
            filterMenu.IsOpen = true;
        }


        private void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            List<Product> products = new List<Product>();

            // Handle DatePicker Filter
            if (DatePicker.Visibility == Visibility.Visible && DatePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = DatePicker.SelectedDate.Value;
                products = _databaseHelper.GetProductsByDate(selectedDate);
            }
            // Handle Name and Username Filters
            else if (FilterTextBox.Visibility == Visibility.Visible && !string.IsNullOrEmpty(FilterTextBox.Text))
            {
                string filterValue = FilterTextBox.Text;
                string filterType = ((ComboBoxItem)FilterTypeComboBox.SelectedItem)?.Content.ToString();

                if (CurrentFilter == "Name")
                {
                    products = _databaseHelper.GetProductsByName(filterValue, filterType);
                }
                else if (CurrentFilter == "Username")
                {
                    products = _databaseHelper.GetProductsByUsername(filterValue, filterType);
                }
            }
            else
            {
                MessageBox.Show("Please select a filter option or enter filter criteria.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Display data in the DataGrid
            if (products.Any())
            {
                DetailsDataGrid.ItemsSource = products;
            }
            else
            {
                MessageBox.Show("No products found based on the filter criteria.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                DetailsDataGrid.ItemsSource = null;
            }
        }



        private void ShowDatePicker()
        {
            DatePicker.Visibility = Visibility.Visible;
            FetchButton.Visibility = Visibility.Visible;
            FilterTypeComboBox.Visibility = Visibility.Collapsed;
            FilterTextBox.Visibility = Visibility.Collapsed;
        }
        private void ShowNameFilter()
        {
            DatePicker.Visibility = Visibility.Collapsed;
            FetchButton.Visibility = Visibility.Visible;
            FilterTextBox.Visibility = Visibility.Visible;
            FilterTypeComboBox.Visibility = Visibility.Visible; // Allow Starts With/Ends With
            FilterTypeComboBox.SelectedIndex = 0; // Default to "Starts With"
            FilterTextBox.Text = string.Empty; // Clear previous filter value
            CurrentFilter = "Name"; // Set current filter to Name
        }

        private void ShowUsernameFilter()
        {
            DatePicker.Visibility = Visibility.Collapsed;
            FetchButton.Visibility = Visibility.Visible;
            FilterTextBox.Visibility = Visibility.Visible;
            FilterTypeComboBox.Visibility = Visibility.Visible; // Allow Starts With/Ends With
            FilterTypeComboBox.SelectedIndex = 0; // Default to "Starts With"
            FilterTextBox.Text = string.Empty; // Clear previous filter value
            CurrentFilter = "Username"; // Set current filter to Username
        }

        private void ShowTransactions_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = DetailsDataGrid.SelectedItem as Product;
            // Navigate to the Show Transactions page
            NavigationService.Navigate(new TransactionPage(selectedProduct.Id));
        }

        private void ShowProductsTransaction_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = DetailsDataGrid.SelectedItem as Product;
            // Navigate to the Show Products Transaction page
            NavigationService.Navigate(new ProductHistoryPage(selectedProduct.Id));
        }
        private void AddProductsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PhotoVault());
        }
       



        //private void ShowNameFilter()
        //{
        //    DatePicker.Visibility = Visibility.Collapsed;
        //    FilterComboBox.Visibility = Visibility.Visible;
        //    FilterTextBox.Visibility = Visibility.Visible;
        //    FilterComboBox.SelectedItem = null; // Reset any previous selection
        //    FilterTextBox.Text = string.Empty;
        //    FilterTextBox.Focus();
        //}
        //private void ShowUserNameFilter()
        //{
        //    DatePicker.Visibility = Visibility.Collapsed;
        //    ClearDateFilter();
        //    FilterComboBox.Visibility = Visibility.Visible;
        //    FilterTextBox.Visibility = Visibility.Visible;
        //    FilterComboBox.SelectedItem = null; // Reset any previous selection
        //    FilterTextBox.Text = string.Empty;
        //    FilterTextBox.Focus();
        //}
        //private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string filterText = FilterTextBox.Text.Trim().ToLower();
        //    string filterOption = (FilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

        //    if (string.IsNullOrEmpty(filterText) || string.IsNullOrEmpty(filterOption))
        //    {
        //        // Clear the displayed results if no input or filter option is selected
        //        ClearResults();
        //        return;
        //    }

        //    // Fetch the data based on the filter and display
        //    List<string> filteredNames = FilterNames(filterText, filterOption);
        //    DisplayFilteredNames(filteredNames);
        //}
        //private void ClearDateFilter()
        //{
        //    // Resetting the DatePicker to null
        //    DatePicker.SelectedDate = null;

        //    // Optionally, clear any associated text field
        //   // DateTextBox.Clear();  // Assuming you have a text box for displaying date
        //}

        //private List<string> FilterNames(string filterText, string filterOption)
        //{
        //    // Example list of names (you can replace it with your actual data retrieval logic)
        //    List<string> allNames = _databaseHelper.LoadNames();
        //    List<string> filteredNames = new List<string>();

        //    if (filterOption == "Starts With")
        //    {
        //        filteredNames = allNames.Where(name => name.ToLower().StartsWith(filterText)).ToList();
        //    }
        //    else if (filterOption == "Ends With")
        //    {
        //        filteredNames = allNames.Where(name => name.ToLower().EndsWith(filterText)).ToList();
        //    }

        //    return filteredNames;
        //}



    }
}
