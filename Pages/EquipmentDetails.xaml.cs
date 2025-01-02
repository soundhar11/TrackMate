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

        public EquipmentDetails()
        {
            InitializeComponent();
            _databaseHelper = new DatabaseHelper("MaterialsTracker.db"); // Provide the correct path
            this.Loaded += OnPageLoaded;
            LoadNamesIntoComboBox();
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
        private void LoadNamesIntoComboBox()
        {
            try
            {
                List<string> names = _databaseHelper.LoadNames();

                // Add the "ALL" option at the beginning of the list
                names.Insert(0, "ALL");

                // Set the updated list as the ComboBox source
                NamesComboBox.ItemsSource = names;
                NamesComboBox.SelectedIndex = 0; // Set "ALL" as the default selected item
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading names: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Handle selection change in ComboBox
        private void NamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NamesComboBox.SelectedItem is string selectedName)
            {
                if (selectedName == "ALL")
                {
                    // Load all data when "ALL" is selected
                    LoadData();
                }
                else
                {
                    // Display details based on the selected name
                    DisplayDetailsForSelectedName(selectedName);
                }
            }
            else
            {
                DetailsDataGrid.ItemsSource = null; // Clear the DataGrid if no selection
            }
        }

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

            // Add "Name" sort option
            MenuItem nameFilter = new MenuItem() { Header = "Name" };
            nameFilter.Click += (s, args) => SortDataGrid("Name");

            // Add "Date" sort option
            MenuItem dateFilter = new MenuItem() { Header = "Date" };
            dateFilter.Click += (s, args) => SortDataGrid("Date");

            // Add "Username" sort option
            MenuItem usernameFilter = new MenuItem() { Header = "Username" };
            usernameFilter.Click += (s, args) => SortDataGrid("Username");

            // Add options to the ContextMenu
            filterMenu.Items.Add(nameFilter);
            filterMenu.Items.Add(dateFilter);
            filterMenu.Items.Add(usernameFilter);

            // Open the ContextMenu
            filterMenu.IsOpen = true;
        }

        private void SortDataGrid(string columnName)
        {
            // Get the DataView from the DataGrid's ItemsSource
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(DetailsDataGrid.ItemsSource);

            if (collectionView != null)
            {
                // Toggle sort direction if the same column is clicked
                _sortDirection = _sortDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;

                // Apply sorting
                collectionView.SortDescriptions.Clear();
                collectionView.SortDescriptions.Add(new SortDescription(columnName, _sortDirection));
            }
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

    }
}
