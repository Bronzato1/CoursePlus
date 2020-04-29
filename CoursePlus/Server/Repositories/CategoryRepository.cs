using CoursePlus.Server.Data;
using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Server.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _dbContext.Categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        }
    }
}
