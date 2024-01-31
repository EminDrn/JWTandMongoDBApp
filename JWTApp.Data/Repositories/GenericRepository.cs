using JWTApp.Core.Repository;
using JWTApp.Data.MongoDbSettings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Data.Repositories
{
    public class GenericRepository<Tentity> : IGenericRepository<Tentity> where Tentity : class
    {
        private readonly IMongoCollection<Tentity> _collection;
        public GenericRepository(IOptions<MongoDbSettings.MongoDbSettings> database, string collectionName)
        {
            var mongoClient = new MongoClient(
                database.Value.ConnectionString);
            var mongoDatbase = mongoClient.GetDatabase(
               database.Value.DatabaseName );

            _collection = mongoDatbase.GetCollection<Tentity>(database.Value.CollectionName);
        }
        public async Task AddAsync(Tentity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<IEnumerable<Tentity>> GetAllAsync()
        {
            var entities = await _collection.Find(_ => true).ToListAsync();
            return entities;
        }

        public async Task<Tentity> GetByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var entity = await _collection.Find(e => e.GetType().GetProperty("Id").GetValue(e).Equals(objectId)).FirstOrDefaultAsync();
            return entity;
        }

        public void Remove(Tentity entity)
        {
            var objectId = entity.GetType().GetProperty("Id").GetValue(entity);
            var filter = Builders<Tentity>.Filter.Eq("Id", objectId);
            _collection.DeleteOne(filter);
        }

        public Tentity Update(Tentity entity)
        {
            var objectId = entity.GetType().GetProperty("Id").GetValue(entity);
            var filter = Builders<Tentity>.Filter.Eq("Id", objectId);
            var result = _collection.ReplaceOne(filter, entity);
            return entity;
        }

        public IQueryable<Tentity> Where(Expression<Func<Tentity, bool>> predicate)
        {
            return _collection.AsQueryable().Where(predicate);
        }
    }
}
