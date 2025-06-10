using FinanceApi.Infrastructure.Abstraction;
using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services
{
    public class SpaceService
    {
        private readonly ISpaceRepository _spaceRepository;
        private readonly ISpaceUserRepository _spaceUserRepository;

        public SpaceService(ISpaceRepository spaceRepository, ISpaceUserRepository spaceUserRepository)
        {
            _spaceRepository = spaceRepository;
            _spaceUserRepository = spaceUserRepository;
        }

        public async Task<Space> CreateSpace(Guid userId, string name, Currency spaceCurrency)
        {

            var space = new Space()
            {
                Name = name,
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Currency = spaceCurrency,
                OwnerId = userId
            };
            //создаем пространство
            await _spaceRepository.CreateAsync(space);

            //добавляем пользователя в пространство
            var spaceUser = new SpaceUser()
            {
                Id = Guid.NewGuid(),
                InviteStatus = "accepted",
                Role = "owner",
                SpaceId = space.Id,
                UserId = userId
            };
            await _spaceUserRepository.CreateAsync(spaceUser);

            return space;
        }

        public async Task<Space> Update(Guid userId, Space space)
        {
            //редактирование разрешено либо владельцу либо пользователю в роли "Editor"
            if (userId != space.OwnerId)
            {
                var spaceUser = await _spaceUserRepository.GetSpaceUserAsync(userId, space.Id);
                if (spaceUser == null || spaceUser.Role != "editor")
                {
                    throw new Exception($"Вы не можете редактировать пространство '{space.Name}'! Текущая роль '{spaceUser.Role}'.");
                }
            }

            await _spaceRepository.UpdateAsync(space.Id, space);
            return space;
        }

        public async Task<bool> Delete(Guid userId, Guid spaceId, bool softDelete = true)
        {
            var space = await _spaceRepository.GetByIdAsync(spaceId);
            if (space is null)
                throw new Exception($"Пространство id: '{spaceId}' не найдено.");
            //редактирование разрешено либо владельцу либо пользователю в роли "Editor"
            if (userId != space.OwnerId)
            {
                throw new Exception($"Вы не можете удалить пространство '{space.Name}'! Это может сделать только владелец пространства.");
            }


            if (softDelete)
            {
                //если мягкое удаление, то просто ставим пометку но не удаляем запись
                space.IsArchived = true;
                space.ArchivedAt = DateTime.UtcNow;
                await _spaceRepository.UpdateAsync(space.Id, space);
            }
            else
            {
                //если полное удаление, то полностью удаляем пространство
                await _spaceRepository.DeleteAsync(spaceId);
                await _spaceUserRepository.DeleteAllSpaceAsync(spaceId);
            }
            return true;
        }

        public async Task<List<Space>> GetUserSpaces(Guid userId)
        {
            var userSpaces = await _spaceUserRepository.GetByUserIdAsync(userId);
            if (userSpaces.Count == 0)
                return null;

            var spacesIds = userSpaces.Select(x => x.SpaceId).ToList();
            var spaces = await _spaceRepository.GetSpacesAsync(spacesIds);
            return spaces;
        }

        //проверка доступа к редактированию спейсов и бюджетов
        public async Task<bool> CheckUserSpaceAccess(Guid userId, Guid spaceId)
        {
            var space = await _spaceRepository.GetByIdAsync(spaceId);
            if (space is null)
                throw new Exception($"Пространство id: '{spaceId}' не найдено.");
            if (userId != space.OwnerId)
            {
                var spaceUser = await _spaceUserRepository.GetSpaceUserAsync(userId, space.Id);
                if (spaceUser == null || spaceUser.Role != "editor")
                {
                    return false;
                }
            }
            return true;
        }
    }
}
