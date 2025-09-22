using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP422_Reshetnyak_Kuvshinova
{
    public class Sale
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount => Price * Quantity;
        public DateTime SaleDate { get; set; }

        public Sale()
        {
            SaleDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{ProductName} (Код: {ProductId}) - {Quantity} шт. × {Price:C} = {TotalAmount:C} - {SaleDate:dd.MM.yyyy HH:mm}";
        }
    }
}
