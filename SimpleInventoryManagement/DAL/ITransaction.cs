using InventoryTransactionsApp.ViewModels;
using InventoryTransactionsApp.Models;

namespace InventoryTransactionsApp.DAL
{
    public interface ITransaction
    {
        Transaction Add(Transaction entity);
        IEnumerable<ViewTransactionInfo> GetProductTransactions();
        Transaction Update(Transaction entity);
        IEnumerable<Product> GetAllProducts();
        IEnumerable<ViewTransactionInfo> GetTransactionsByProductName(string productName);
        ViewTransactionInfo GetByIdJoin(int id);
    }
}
