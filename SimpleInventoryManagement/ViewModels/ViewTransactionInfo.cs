using InventoryTransactionsApp.Models;
using static InventoryTransactionsApp.Models.Transaction;
using static InventoryTransactionsApp.Models.Product;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryTransactionsApp.ViewModels
{
    public class ViewTransactionInfo
    {
        public int TransactionId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        [Column("TransactionType")]
        public TransactionType Type { get; set; }

        public int? Quantity { get; set; }
        public DateTime? Date { get; set; }
        public IEnumerable<Product>? Products { get; set; } // Add this property

        public int StockLevel { get; set; }
    }
}