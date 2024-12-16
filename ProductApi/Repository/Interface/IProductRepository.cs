using ProductApi.Models;

namespace ProductApi.Repository.Interface
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        ICollection<Product> GetProductsByCategory(int categoryId);
        IEnumerable<Product> SearchProducts( string title);
        Product GetProduct(int productId);
        bool ProductExists(int productId);
        bool ProductExists(string title);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        bool Save();
    }
}
