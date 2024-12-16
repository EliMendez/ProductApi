using ProductApi.Data;
using ProductApi.Models;
using ProductApi.Repository.Interface;

namespace ProductApi.Repository.Service
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ProductExists(int productId)
        {
            return _db.Products.Any(p => p.Id == productId);
        }

        public bool ProductExists(string title)
        {
            return _db.Products.Any(p => p.Title.ToLower().Trim() == title.ToLower().Trim());
        }

        public bool CreateProduct(Product product)
        {
            product.CreatedDate = DateTime.Now;
            _db.Products.Add(product);

            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _db.Products.Remove(product);

            return Save();
        }

        public ICollection<Product> GetProducts()
        {
            return _db.Products.OrderBy(p => p.Title).ToList();
        }

        public Product GetProduct(int productId)
        {
            return _db.Products.FirstOrDefault(p => p.Id == productId);
        }

        public bool Save()
        {
            bool valor = _db.SaveChanges() >= 0 ? true : false;
            return valor;
        }

        public bool UpdateProduct(Product product)
        {
            product.UpdatedDate = DateTime.Now;

            var productExists = _db.Products.Find(product.Id);

            if (productExists != null)
            {
                _db.Entry(productExists).CurrentValues.SetValues(product);
            }
            else
            {
                _db.Products.Update(product);
            }

            return Save();
        }
    }
}
