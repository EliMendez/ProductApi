using ProductApi.Data;
using ProductApi.Models;
using ProductApi.Repository.Interface;

namespace ProductApi.Repository.Service
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool categoryExists(int categoryId)
        {
            return _db.Categories.Any(c => c.Id == categoryId);
        }

        public bool categoryExists(string name)
        {
            return _db.Categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool createCategory(Category category)
        {
            category.CreatedDate = DateTime.Now;
            _db.Categories.Add(category);

            return save();
        }

        public bool deleteCategory(Category category)
        {
            _db.Categories.Remove(category);

            return save();
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _db.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public bool save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool updateCategory(Category category)
        {
            category.UpdatedDate = DateTime.Now;
            _db.Categories.Update(category);

            return save();
        }
    }
}
