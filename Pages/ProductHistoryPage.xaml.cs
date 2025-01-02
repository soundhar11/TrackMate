using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrackMate.Database;
using static TrackMate.Database.ProductHistory;

namespace TrackMate.Pages
{
    /// <summary>
    /// Interaction logic for ProductHistoryPage.xaml
    /// </summary>
    /// 
    public partial class ProductHistoryPage : Page
    {
        private ProductHistory _productHistory;
        private int _productId;
        public ProductHistoryPage(int productId)
        {
            InitializeComponent();
            _productHistory = new ProductHistory("MaterialsTracker.db"); // Provide the correct path
            _productId = productId;
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                // Retrieve product details by name
                List<HistoryModel> TransactionModels = _productHistory.GetHistoryByOriginalId(_productId);

                if (TransactionModels.Any())
                {
                    // Display data in the DataGrid
                    DetailsDataGrid.ItemsSource = TransactionModels;
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
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
