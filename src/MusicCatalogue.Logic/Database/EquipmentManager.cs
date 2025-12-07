using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    public class EquipmentManager : DatabaseManagerBase, IEquipmentManager
    {
        internal EquipmentManager(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Return the first item of equipment matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Equipment> GetAsync(Expression<Func<Equipment, bool>> predicate)
        {
            List<Equipment> equipment = await ListAsync(predicate);

#pragma warning disable CS8603
            return equipment.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all items of equipment matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<Equipment>> ListAsync(Expression<Func<Equipment, bool>> predicate)
            => await Context.Equipment
                            .Include(x => x.Manufacturer)
                            .Include(x => x.EquipmentType)
                            .Include(x => x.Retailer)
                            .Where(predicate)
                            .OrderBy(x => x.Description)
                            .ToListAsync();

        /// <summary>
        /// Add an item of equipment
        /// </summary>
        /// <param name="equipmentTypeId"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="description"></param>
        /// <param name="model"></param>
        /// <param name="serialNumber"></param>
        /// <param name="isWishListItem"></param>
        /// <param name="purchased"></param>
        /// <param name="price"></param>
        /// <param name="retailerId"></param>
        /// <returns></returns>
        public async Task<Equipment> AddAsync(
            int equipmentTypeId,
            int manufacturerId,
            string description,
            string? model,
            string? serialNumber,
            bool? isWishListItem,
            DateTime? purchased,
            decimal? price,
            int? retailerId)
        {
            // Create the equipment instance
            var equipment = new Equipment
            {
                EquipmentTypeId = equipmentTypeId,
                ManufacturerId = manufacturerId,
                Description = StringCleaner.RemoveInvalidCharacters(description)!,
                Model = StringCleaner.RemoveInvalidCharacters(model),
                SerialNumber = StringCleaner.RemoveInvalidCharacters(serialNumber),
                IsWishListItem = isWishListItem,
                Purchased = purchased,
                Price = price,
                RetailerId = retailerId
            };

            // Add it and save changes
            await Context.Equipment.AddAsync(equipment);
            await Context.SaveChangesAsync();

            // Reload it to load related entities
            equipment = await GetAsync(x => x.Id == equipment.Id);

            return equipment;
        }

        /// <summary>
        /// Update the properties of the specified item of equipment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equipmentTypeId"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="description"></param>
        /// <param name="model"></param>
        /// <param name="serialNumber"></param>
        /// <param name="isWishListItem"></param>
        /// <param name="purchased"></param>
        /// <param name="price"></param>
        /// <param name="retailerId"></param>
        /// <returns></returns>
        public async Task<Equipment?> UpdateAsync(
            int id,
            int equipmentTypeId,
            int manufacturerId,
            string description,
            string? model,
            string? serialNumber,
            bool? isWishListItem,
            DateTime? purchased,
            decimal? price,
            int? retailerId)
        {
            var equipment = Context.Equipment.FirstOrDefault(x => x.Id == id);
            if (equipment != null)
            {
                // Update the entity and save changes
                equipment.EquipmentTypeId = equipmentTypeId;
                equipment.ManufacturerId = manufacturerId;
                equipment.Description = StringCleaner.RemoveInvalidCharacters(description)!;
                equipment.Model = StringCleaner.RemoveInvalidCharacters(model);
                equipment.SerialNumber = StringCleaner.RemoveInvalidCharacters(serialNumber);
                equipment.IsWishListItem = isWishListItem;
                equipment.Purchased = purchased;
                equipment.Price = price;
                equipment.RetailerId = retailerId;
                await Context.SaveChangesAsync();

                // Reload it to load related entities
                equipment = await GetAsync(x => x.Id == equipment.Id);
            }

            return equipment;
        }

        /// <summary>
        /// Delete the item of equipment with the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            // Find the equipment record and check it exists
            var equipment = await GetAsync(x => x.Id == id);
            if (equipment != null)
            {
                // Delete the equipment record and save changes
                Factory.Context.Remove(equipment);
                await Factory.Context.SaveChangesAsync();
            }
        }
    }
}
