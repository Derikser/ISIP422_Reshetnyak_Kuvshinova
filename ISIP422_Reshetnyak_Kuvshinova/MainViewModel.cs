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

            // Команды
            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);
            SupplyProductCommand = new RelayCommand(SupplyProduct, CanModifyProduct);
            SellProductCommand = new RelayCommand(SellProduct, CanModifyProduct);
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
                SelectedProduct.Quantity += 10; // Поставка 10 единиц
                MessageBox.Show($"Поставка выполнена! Товар: {SelectedProduct.Name}\nНовое количество: {SelectedProduct.Quantity}", "Поставка товара", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SellProduct()
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.Quantity > 0)
                {
                    SelectedProduct.Quantity -= 1; // Продажа 1 единицы
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