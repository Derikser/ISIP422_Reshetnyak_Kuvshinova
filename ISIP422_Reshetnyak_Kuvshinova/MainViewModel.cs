using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ISIP422_Reshetnyak_Kuvshinova
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _products;
        private Product _selectedProduct;

        public MainViewModel()
        {
            Products = new ObservableCollection<Product>();
            AddTestData(); // Добавляем тестовые данные

            // Команды
            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);
            SupplyProductCommand = new RelayCommand(SupplyProduct, CanModifyProduct);
            SellProductCommand = new RelayCommand(SellProduct, CanModifyProduct);
        }

        private void AddTestData()
        {
            // 5 тестовых товаров
            Products.Add(new Product { Name = "Ноутбук HP", Price = 55000, Quantity = 5, Category = Category.Electronics });
            Products.Add(new Product { Name = "Яблоки", Price = 120, Quantity = 50, Category = Category.Food });
            Products.Add(new Product { Name = "Футболка", Price = 1500, Quantity = 0, Category = Category.Clothing });
            Products.Add(new Product { Name = "Наушники", Price = 3500, Quantity = 8, Category = Category.Electronics });
            Products.Add(new Product { Name = "Шоколад", Price = 80, Quantity = 25, Category = Category.Food });
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

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        // Команды
        public RelayCommand AddProductCommand { get; }
        public RelayCommand DeleteProductCommand { get; }
        public RelayCommand SupplyProductCommand { get; }
        public RelayCommand SellProductCommand { get; }

        private void AddProduct()
        {
            var newProduct = new Product()
            {
                Name = "Новый товар",
                Price = 100,
                Quantity = 0,
                Category = Category.Electronics
            };
            Products.Add(newProduct);
            SelectedProduct = newProduct;
        }

        private void DeleteProduct()
        {
            if (SelectedProduct != null)
            {
                Products.Remove(SelectedProduct);
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
                    SelectedProduct.Quantity -= 1;
                    MessageBox.Show($"Продажа выполнена! Товар: {SelectedProduct.Name}\nОстаток: {SelectedProduct.Quantity}", "Продажа товара", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Недостаточно товара на складе!\nТовар: {SelectedProduct.Name}", "Ошибка продажи", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private bool CanDeleteProduct()
        {
            return SelectedProduct != null;
        }

        private bool CanModifyProduct()
        {
            return SelectedProduct != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}