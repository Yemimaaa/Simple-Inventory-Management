using InventoryTransactionsApp.Models;
namespace InventoryTransactionsApp.DAL
{
    public interface IProduct : ICrud<Product>
    {
        IEnumerable<Product> GetProductsByName(string productName);
    }
}
