using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    public class EquipmentTypeManager : DatabaseManagerBase, IEquipmentTypeManager
    {
        internal EquipmentTypeManager(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Return the first equipment type matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<EquipmentType> GetAsync(Expression<Func<EquipmentType, bool>> predicate)
        {
            List<EquipmentType> equipmentTypes = await ListAsync(predicate);

#pragma warning disable CS8603
            return equipmentTypes.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all equipment types matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<EquipmentType>> ListAsync(Expression<Func<EquipmentType, bool>> predicate)
            => await Context.EquipmentTypes
                            .Where(predicate)
                            .OrderBy(x => x.Name)
                            .ToListAsync();

        /// <summary>
        /// Add an equipment type, if they doesn't already exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<EquipmentType> AddAsync(string name)
        {
            var clean = StringCleaner.Clean(name)!;
            var equipmentType = await GetAsync(a => a.Name == clean);

            if (equipmentType == null)
            {
                equipmentType = new EquipmentType
                {
                    Name = clean
                };

                await Context.EquipmentTypes.AddAsync(equipmentType);
                await Context.SaveChangesAsync();
            }

            return equipmentType;
        }

        /// <summary>
        /// Update the properties of the specified equipment type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<EquipmentType?> UpdateAsync(int id, string name)
        {
            var equipmentType = Context.EquipmentTypes.FirstOrDefault(x => x.Id == id);
            if (equipmentType != null)
            {
                // Save the changes
                equipmentType.Name = StringCleaner.Clean(name)!;
                await Context.SaveChangesAsync();
            }

            return equipmentType;
        }

        /// <summary>
        /// Delete the equipment type with the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            // Find the equipment type record and check it exists
            var equipmentType = await GetAsync(x => x.Id == id);
            if (equipmentType != null)
            {
                // If there's any equipment associated with the type, it can't be deleted
                var equipment = Context.Equipment.Where(x => x.EquipmentTypeId == id);
                if (equipment.Any())
                {
                    var message = $"Cannot delete equipment type '{equipmentType.Name} with Id {id} with equipment associated with it";
                    throw new EquipmentTypeInUseException(message);
                }

                // Delete the equipment type record and save changes
                Factory.Context.Remove(equipmentType);
                await Factory.Context.SaveChangesAsync();
            }
        }
    }
}