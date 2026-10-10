using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.core.Entities
{
    public class ProductType : BaseEntity
    {
        [BsonElement("Name")]
        public string Name { get; set; }
    }
}
