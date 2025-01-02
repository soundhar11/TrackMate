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

namespace TrackMate.Pages
{
    /// <summary>
    /// Interaction logic for TransactionPage.xaml
    /// </summary>
    public partial class TransactionPage : Page
    {
        private Transaction _transaction;
        private int _productId;

        public TransactionPage(int productId)
        {
            InitializeComponent();
            _transaction = new Transaction("MaterialsTracker.db"); // Provide the correct path
            _productId = productId;
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                // Retrieve product details by name
                List<TransactionModel> TransactionModels = _transaction.GetTransactionsByMachineryId(_productId);

                if (TransactionModels.Any())
                {
                    // Display data in the DataGrid
                    DetailsDataGrid.ItemsSource = TransactionModels;
                }
               
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
