using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.Logic.Database
{
    public class ManufacturerManager : DatabaseManagerBase, IManufacturerManager
    {
        internal ManufacturerManager(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Return the first manufacturer matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Manufacturer> GetAsync(Expression<Func<Manufacturer, bool>> predicate)
        {
            List<Manufacturer> manufacturers = await ListAsync(predicate);

#pragma warning disable CS8603
            return manufacturers.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all manufacturers matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<Manufacturer>> ListAsync(Expression<Func<Manufacturer, bool>> predicate)
            => await Context.Manufacturers
                            .Where(predicate)
                            .OrderBy(x => x.Name)
                            .ToListAsync();

        /// <summary>
        /// Add a manufacturer, if they doesn't already exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Manufacturer> AddAsync(string name)
        {
            var clean = StringCleaner.Clean(name)!;
            var manufacturer = await GetAsync(a => a.Name == clean);

            if (manufacturer == null)
            {
                // Get a serchable name
                manufacturer = new Manufacturer
                {
                    Name = clean
                };

                await Context.Manufacturers.AddAsync(manufacturer);
                await Context.SaveChangesAsync();
            }

            return manufacturer;
        }

        /// <summary>
        /// Update the properties of the specified manufacturer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Manufacturer?> UpdateAsync(int id, string name)
        {
            var manufacturer = Context.Manufacturers.FirstOrDefault(x => x.Id == id);
            if (manufacturer != null)
            {
                // Save the changes
                manufacturer.Name = StringCleaner.Clean(name)!;
                await Context.SaveChangesAsync();
            }

            return manufacturer;
        }

        /// <summary>
        /// Delete the manufacturer with the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            // Find the manufacturer record and check it exists
            var manufacturer = await GetAsync(x => x.Id == id);
            if (manufacturer != null)
            {
                // If there's any equipment associated with the manufacturer, they can't be deleted
                var equipment = Context.Equipment.Where(x => x.ManufacturerId == id);
                if (equipment.Any())
                {
                    var message = $"Cannot delete manufacturer '{manufacturer.Name} with Id {id} with equipment associated with them";
                    throw new ManufacturerInUseException(message);
                }

                // Delete the manufacturer record and save changes
                Factory.Context.Remove(manufacturer);
                await Factory.Context.SaveChangesAsync();
            }
        }
    }
}