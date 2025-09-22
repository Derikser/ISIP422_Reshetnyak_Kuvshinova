using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Collections.Generic;

namespace ISIP422_Reshetnyak_Kuvshinova
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _products;
        private ObservableCollection<Product> _filteredProducts;
        private Product _selectedProduct;
        private string _searchText;
        private List<Product> _allProducts;
        private SalesHistory _salesHistory;

        public MainViewModel()
        {
            _allProducts = new List<Product>();
            Products = new ObservableCollection<Product>();
            FilteredProducts = new ObservableCollection<Product>();
            _salesHistory = new SalesHistory();

            AddTestData();

            // Команды
            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);
            SupplyProductCommand = new RelayCommand(SupplyProduct, CanModifyProduct);
            SellProductCommand = new RelayCommand(SellProduct, CanModifyProduct);
            SearchProductsCommand = new RelayCommand(SearchProducts);
            UndoLastSaleCommand = new RelayCommand(UndoLastSale, CanUndoLastSale);
            ShowSalesReportCommand = new RelayCommand(ShowSalesReport);
        }

        private void AddTestData()
        {
            // 5 тестовых товаров
            _allProducts.Add(new Product { Name = "Ноутбук HP", Price = 55000, Quantity = 5, Category = Category.Electronics });
            _allProducts.Add(new Product { Name = "Яблоки", Price = 120, Quantity = 50, Category = Category.Food });
            _allProducts.Add(new Product { Name = "Футболка", Price = 1500, Quantity = 0, Category = Category.Clothing });
            _allProducts.Add(new Product { Name = "Наушники", Price = 3500, Quantity = 8, Category = Category.Electronics });
            _allProducts.Add(new Product { Name = "Шоколад", Price = 80, Quantity = 25, Category = Category.Food });

            UpdateFilteredProducts();
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ObservableCollection<Product> FilteredProducts
        {
            get => _filteredProducts;
            set
            {
                _filteredProducts = value;
                OnPropertyChanged(nameof(FilteredProducts));
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                // Обновляем состояние команд, зависящих от выбранного товара
                DeleteProductCommand.RaiseCanExecuteChanged();
                SupplyProductCommand.RaiseCanExecuteChanged();
                SellProductCommand.RaiseCanExecuteChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        // Команды
        public RelayCommand AddProductCommand { get; }
        public RelayCommand DeleteProductCommand { get; }
        public RelayCommand SupplyProductCommand { get; }
        public RelayCommand SellProductCommand { get; }
        public RelayCommand SearchProductsCommand { get; }
        public RelayCommand UndoLastSaleCommand { get; }
        public RelayCommand ShowSalesReportCommand { get; }

        private void AddProduct()
        {
            var newProduct = new Product()
            {
                Name = "Новый товар",
                Price = 100,
                Quantity = 0,
                Category = Category.Electronics
            };
            _allProducts.Add(newProduct);
            UpdateFilteredProducts();
            SelectedProduct = newProduct;
        }

        private void DeleteProduct()
        {
            if (SelectedProduct != null)
            {
                _allProducts.Remove(SelectedProduct);
                UpdateFilteredProducts();
            }
        }

        private void SupplyProduct()
        {
            if (SelectedProduct != null)
            {
                SelectedProduct.Quantity += 10;
                MessageBox.Show($"Поставка выполнена! Товар: {SelectedProduct.Name}\nНовое количество: {SelectedProduct.Quantity}", "Поставка товара", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SellProduct()
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.Quantity > 0)
                {
                    // Создаем запись о продаже
                    var sale = new Sale
                    {
                        ProductId = SelectedProduct.Id,
                        ProductName = SelectedProduct.Name,
                        Price = SelectedProduct.Price,
                        Quantity = 1
                    };

                    _salesHistory.AddSale(sale);
                    SelectedProduct.Quantity -= 1;

                    // Обновляем состояние команды отмены
                    UndoLastSaleCommand.RaiseCanExecuteChanged();

                    MessageBox.Show($"Продажа выполнена! Товар: {SelectedProduct.Name}\nОстаток: {SelectedProduct.Quantity}", "Продажа товара", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Недостаточно товара на складе!\nТовар: {SelectedProduct.Name}", "Ошибка продажи", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void UndoLastSale()
        {
            var lastSale = _salesHistory.UndoLastSale();
            if (lastSale != null)
            {
                // Возвращаем товар на склад
                var product = _allProducts.FirstOrDefault(p => p.Id == lastSale.ProductId);
                if (product != null)
                {
                    product.Quantity += lastSale.Quantity;
                    MessageBox.Show($"Продажа отменена! Товар возвращен на склад: {product.Name}\nНовое количество: {product.Quantity}", "Отмена продажи", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Обновляем состояние команды отмены
                UndoLastSaleCommand.RaiseCanExecuteChanged();
            }
        }

        private void ShowSalesReport()
        {
            var sales = _salesHistory.GetAllSales();
            if (sales.Count == 0)
            {
                MessageBox.Show("История продаж пуста.", "Отчёт о продажах", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string report = "ОТЧЁТ О ПРОДАЖАХ\n\n";
            report += "Всего продаж: " + sales.Count + "\n";
            report += "Общее количество проданных товаров: " + _salesHistory.GetTotalSoldQuantity() + " шт.\n";
            report += "Общая сумма продаж: " + _salesHistory.GetTotalSalesAmount().ToString("C") + "\n\n";
            report += "Детализация:\n";

            foreach (var sale in sales)
            {
                report += $"• {sale}\n";
            }

            MessageBox.Show(report, "Отчёт о продажах", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SearchProducts()
        {
            UpdateFilteredProducts();
        }

        private void UpdateFilteredProducts()
        {
            var filtered = _allProducts;

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = _allProducts.Where(p =>
                    p.Id.ToString().Contains(SearchText) ||
                    p.Name.ToLower().Contains(SearchText.ToLower()) ||
                    p.Category.ToString().ToLower().Contains(SearchText.ToLower())
                ).ToList();
            }

            FilteredProducts = new ObservableCollection<Product>(filtered);
            Products = FilteredProducts;
        }

        private bool CanDeleteProduct()
        {
            return SelectedProduct != null;
        }

        private bool CanModifyProduct()
        {
            return SelectedProduct != null;
        }

        private bool CanUndoLastSale()
        {
            return _salesHistory.CanUndo();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}