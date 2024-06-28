using InventoryTransactionsApp.Models;
using Microsoft.EntityFrameworkCore;
using InventoryTransactionsApp.ViewModels;
using InventoryTransactionsApp.DAL;

namespace InventoryTransactionsApp.EF
{
    public class TransactionEF : ITransaction
    {
        private readonly InventoryDbContext _context;

        public TransactionEF(InventoryDbContext context)
        {
            _context = context;
        }

        public Transaction Add(Transaction entity)
        {
            try
            {
                ValidateTransactionType(entity.Type);

                var product = _context.Products.FirstOrDefault(p => p.ProductId == entity.ProductId);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                if (entity.Type == Transaction.TransactionType.Remove && product.StockLevel < entity.Quantity)
                {
                    throw new Exception("Can't remove more stock than available");
                }

                _context.Transactions.Add(entity);

                if (entity.Type == Transaction.TransactionType.Add)
                {
                    product.StockLevel += entity.Quantity;
                }
                else if (entity.Type == Transaction.TransactionType.Remove)
                {
                    product.StockLevel -= entity.Quantity;
                }

                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ViewTransactionInfo GetByIdJoin(int id)
        {
            var result = _context.Transactions
                .Where(t => t.TransactionId == id)
                .Include(t => t.Product)
                .FirstOrDefault();

            if (result == null)
            {
                throw new ArgumentException("Transaction not found");
            }

            // Mapping to TransactionProductViewModel
            var viewModel = new ViewTransactionInfo
            {
                TransactionId = result.TransactionId,
                ProductId = result.ProductId,
                ProductName = result.Product.Name,
                Type = result.Type,
                Quantity = result.Quantity,
                Date = result.Date,
                StockLevel = result.Product.StockLevel
            };

            return viewModel;
        }
        

        public IEnumerable<ViewTransactionInfo> GetProductTransactions()
        {
            var results = from t in _context.Transactions
                          join p in _context.Products on t.ProductId equals p.ProductId
                          select new ViewTransactionInfo
                          {
                              TransactionId = t.TransactionId,
                              ProductId = t.ProductId,
                              ProductName = p.Name,
                              Type = t.Type,
                              Quantity = t.Quantity,
                              Date = t.Date
                          };
            return results.ToList();
        }

        public Transaction Update(Transaction entity)
        {
            try
            {
                ValidateTransactionType(entity.Type);

                var transaction = _context.Transactions.Find(entity.TransactionId);
                if (transaction == null)
                {
                    throw new Exception("Transaction not found");
                }

                var product = _context.Products.FirstOrDefault(p => p.ProductId == entity.ProductId);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                if (transaction.Type == Transaction.TransactionType.Add)
                {
                    product.StockLevel -= transaction.Quantity;
                }
                else if (transaction.Type == Transaction.TransactionType.Remove)
                {
                    product.StockLevel += transaction.Quantity;
                }

                if (entity.Type == Transaction.TransactionType.Add)
                {
                    product.StockLevel += entity.Quantity;
                }
                else if (entity.Type == Transaction.TransactionType.Remove)
                {
                    product.StockLevel -= entity.Quantity;
                }

                transaction.Type = entity.Type;
                transaction.Quantity = entity.Quantity;
                transaction.Date = entity.Date;

                _context.SaveChanges();
                return transaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ViewTransactionInfo> GetTransactionsByProductName(string productName)
        {
            var results = from t in _context.Transactions
                          join p in _context.Products on t.ProductId equals p.ProductId
                          where p.Name.Contains(productName)
                          select new ViewTransactionInfo
                          {
                              TransactionId = t.TransactionId,
                              ProductId = t.ProductId,
                              Type = t.Type,
                              Quantity = t.Quantity,
                              Date = t.Date,
                              ProductName = p.Name,
                              StockLevel = p.StockLevel
                          };

            return results.ToList();
        }

        private void ValidateTransactionType(Transaction.TransactionType transactionType)
        {
            if (!Enum.IsDefined(typeof(Transaction.TransactionType), transactionType))
            {
                throw new ArgumentException("Invalid transaction type");
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
    }
}