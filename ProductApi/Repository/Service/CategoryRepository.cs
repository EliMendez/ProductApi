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

        public bool CategoryExists(int categoryId)
        {
            return _db.Categories.Any(c => c.Id == categoryId);
        }

        public bool CategoryExists(string name)
        {
            return _db.Categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool CreateCategory(Category category)
        {
            category.CreatedDate = DateTime.Now;
            _db.Categories.Add(category);

            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _db.Categories.Remove(category);

            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _db.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public bool Save()
        {
            bool valor = _db.SaveChanges() >= 0 ? true : false;
            return valor;
        }

        public bool UpdateCategory(Category category)
        {
            category.UpdatedDate = DateTime.Now;

            var categoryExists = _db.Categories.Find(category.Id);

            if (categoryExists != null)
            {
                _db.Entry(categoryExists).CurrentValues.SetValues(category);
            }
            else
            {
                _db.Categories.Update(category);
            }

            return Save();
        }
    }
}
