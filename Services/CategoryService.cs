using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wallets.BusinessLayer;
using Wallets.DataStorage;

namespace Wallets.Services
{
    public class CategoryService
    {
        private FileDataStorage<Category> _storage = new FileDataStorage<Category>();



        public void DeleteCategory(Category category)
        {
            Thread.Sleep(1000);
            _storage.Delete(category);
        }


        public async Task<bool> AddOrUpdateCategoryAsync(Category category)
        {
            Thread.Sleep(1000);
            await Task.Run(() => _storage.AddOrUpdateAsync(category));
            return true;
        }


        public List<Category> GetCategories()
        {
            Task<List<Category>> categories = Task.Run<List<Category>>(async () => await _storage.GetAllAsync());
            return categories.Result;
        }

    }
}
