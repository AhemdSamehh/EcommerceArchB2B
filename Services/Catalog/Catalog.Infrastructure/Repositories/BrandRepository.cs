using Catalog.core.Entities;
using Catalog.core.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Catalog.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IMongoCollection<ProductBrand> _brands;

        public BrandRepository(IOptions<DatabaseSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _brands = database.GetCollection<ProductBrand>(settings.BrandCollectionName);
        }
        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        => await _brands.Find(brand => true).ToListAsync();

        public async Task<ProductBrand> GetBrandById(string id)
        => await _brands.Find(brand => brand.Id == id).FirstOrDefaultAsync();
    }
}
