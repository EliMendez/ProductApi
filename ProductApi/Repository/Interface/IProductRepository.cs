using ProductApi.Models;

namespace ProductApi.Repository.Interface
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        Product GetProduct(int productId);
        bool ProductExists(int productId);
        bool ProductExists(string title);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        bool Save();
    }
}
