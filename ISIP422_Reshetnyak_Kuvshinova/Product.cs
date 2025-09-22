using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ISIP422_Reshetnyak_Kuvshinova
{
    public class Product : INotifyPropertyChanged
    {
        private static int _lastProductId = 0;

        public Product()
        {
            _lastProductId++;
            Id = _lastProductId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsInStock => Quantity > 0;
        public Category Category { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum Category
    {
        Electronics,
        Food,
        Clothing
    }
}