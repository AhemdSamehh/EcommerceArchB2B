using Catalog.core.Entities;
using Catalog.core.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private readonly IMongoCollection<ProductType> _types;
        public TypeRepository(IOptions<DatabaseSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _types = database.GetCollection<ProductType>(settings.TypeCollectionName);

        }
        public async Task<IEnumerable<ProductType>> GetAllTypes()
        => await _types.Find(_ => true).ToListAsync();

        public async Task<ProductType> GetByIdAsync(string id)
        => await _types.Find(type => type.Id == id).FirstOrDefaultAsync();
    }
}
