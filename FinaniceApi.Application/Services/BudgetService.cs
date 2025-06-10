using FinanceApi.Infrastructure.Abstraction;
using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services
{
    public class BudgetService
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly SpaceService _spaceService;

        public BudgetService(IBudgetRepository budgetRepository, SpaceService spaceService)
        {
            _budgetRepository = budgetRepository;
            _spaceService = spaceService;
        }

        public async Task<Budget> Create(Guid spaceId, Currency currency, string name, Guid? parentId = null)
        {
            var budget = new Budget()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                HideInShared = false,
                IsSystem = true,
                SortOrder = 0,
                ParentId = parentId.HasValue ? parentId.Value : null,
                CurrentBalance = 0,
                IncludeChildren = true,
                CanBeMinus = true,
                SpaceId = spaceId,
                Currency = currency,
                Name = name
            };
            await _budgetRepository.CreateAsync(budget);
            return budget;
        }

        public async Task<Budget> CreateDefaultBudget(Guid spaceId, Currency currency)
        {
            string mainBudgetName = "Мой бюджет";
            return await Create(spaceId, currency, mainBudgetName, null);
        }

        public async Task<Budget> Update(Guid userId, Budget updatedBudget)
        {
            if (!await _spaceService.CheckUserSpaceAccess(userId, updatedBudget.SpaceId))
            {
                throw new Exception($"У вас нет прав на редактирование бюджетов в этом пространстве.");
            }
            await _budgetRepository.UpdateAsync(updatedBudget.Id, updatedBudget);
            return updatedBudget;
        }

        public async Task<bool> Delete(Guid userId, Guid spaceId, Guid budgetId)
        {
            if (!await _spaceService.CheckUserSpaceAccess(userId, spaceId))
            {
                throw new Exception($"У вас нет прав на редактирование бюджетов в этом пространстве.");
            }
            await _budgetRepository.DeleteAsync(budgetId);
            return true;
        }

        public async Task<List<Budget>> GetSpaceBudgets(Guid spaceId)
        {
            var spaceBudgets = await _budgetRepository.GetBySpaceIdAsync(spaceId);
            return spaceBudgets;
        }
    }
}
