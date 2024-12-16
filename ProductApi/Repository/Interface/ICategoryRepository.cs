using ProductApi.Models;

namespace ProductApi.Repository.Interface
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        bool categoryExists(int categoryId);
        bool categoryExists(string name);
        bool createCategory(Category category);
        bool updateCategory(Category category);
        bool deleteCategory(Category category);
        bool save();
    }
}
