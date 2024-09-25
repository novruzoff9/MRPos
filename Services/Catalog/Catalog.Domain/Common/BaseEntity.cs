using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Common;

public class BaseEntity
{
    //[BsonId]
    //[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    //TODO: MongoDb-e kecende burani string ele
    public int Id { get; set; }
}
