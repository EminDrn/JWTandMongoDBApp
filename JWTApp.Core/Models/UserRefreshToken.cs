using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Core.Models
{
    public  class UserRefreshToken
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public string Code { get; set; }

        public DateTime Expiration { get; set; }
    }
}
