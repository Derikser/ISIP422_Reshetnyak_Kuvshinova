using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP422_Reshetnyak_Kuvshinova
{
    public class SalesHistory
    {
        private Stack<Sale> _salesStack;
        private List<Sale> _allSales;

        public SalesHistory()
        {
            _salesStack = new Stack<Sale>();
            _allSales = new List<Sale>();
        }

        // Добавление продажи в историю
        public void AddSale(Sale sale)
        {
            _salesStack.Push(sale);
            _allSales.Add(sale);
        }

        // Отмена последней продажи
        public Sale UndoLastSale()
        {
            if (_salesStack.Count > 0)
            {
                var lastSale = _salesStack.Pop();
                _allSales.Remove(lastSale);
                return lastSale;
            }
            return null;
        }

        // Получение всех продаж для отчета
        public List<Sale> GetAllSales()
        {
            return new List<Sale>(_allSales);
        }

        // Получение общей суммы продаж
        public decimal GetTotalSalesAmount()
        {
            return _allSales.Sum(sale => sale.TotalAmount);
        }

        // Получение общего количества проданных товаров
        public int GetTotalSoldQuantity()
        {
            return _allSales.Sum(sale => sale.Quantity);
        }

        // Проверка, есть ли продажи для отмены
        public bool CanUndo()
        {
            return _salesStack.Count > 0;
        }

        // Количество продаж в истории
        public int SalesCount => _salesStack.Count;
    }
}
