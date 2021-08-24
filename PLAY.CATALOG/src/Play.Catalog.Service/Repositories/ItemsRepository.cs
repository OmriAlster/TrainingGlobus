using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    public class ItemsRepository
    {
        private const string collectionName = "items";
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        private readonly IMongoCollection<Item> dbCollection;

        public ItemsRepository(){
            var mongoClient = new MongoClient("https://localhost:27017");
            var dataBase = mongoClient.GetDatabase("items");
            dbCollection = dataBase.GetCollection<Item>(collectionName);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync(){
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetByIdAsync(Guid id){
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id ,id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item item){
            if (item == null)
                throw new NullReferenceException(nameof(item));

            await dbCollection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(Item item){
            if (item == null)
                throw new NullReferenceException(nameof(item));

            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id,item.Id);
            await dbCollection.ReplaceOneAsync(filter,item);
        }

        public async Task DeleteAsync(Guid id){
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id,id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}