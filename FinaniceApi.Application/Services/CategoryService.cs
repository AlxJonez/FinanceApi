using FinanceApi.Infrastructure.Abstraction;
using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Получение всех категорий 
        /// </summary>
        /// <param name="userId">Если передан пользователь</param>
        /// <returns></returns>
        public async Task<List<Category>> GetAllForUserCategories(Guid? userId = null)
        {
            var categories = await _categoryRepository.GetAllForUserAsync(userId);
            return categories;
        }

        //редактирование/удаление

        //изменить стиль категории

        //получить список всех категорий

        //получить список всех вложенных категорий для категорий

    }
}
