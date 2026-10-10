using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Settings
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string BrandCollectionName { get; set; }
        public string TypeCollectionName { get; set; }
        public string ProductCollectionName { get; set; }
    }
}
