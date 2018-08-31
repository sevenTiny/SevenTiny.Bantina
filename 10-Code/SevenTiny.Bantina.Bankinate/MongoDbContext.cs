﻿/*********************************************************
 * CopyRight: 7TINY CODE BUILDER. 
 * Version: 5.0.0
 * Author: 7tiny
 * Address: Earth
 * Create: 2018-04-19 23:58:08
 * Modify: 2018-04-19 23:58:08
 * E-mail: dong@7tiny.com | sevenTiny@foxmail.com 
 * GitHub: https://github.com/sevenTiny 
 * Personal web site: http://www.7tiny.com 
 * Technical WebSit: http://www.cnblogs.com/7tiny/ 
 * Description: 
 * Thx , Best Regards ~
 *********************************************************/
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Bantina.Bankinate
{
    public abstract class MongoDbContext<TDataBase> : IDbContext where TDataBase : class
    {
        public MongoDbContext(string connectionString)
        {
            Client = new MongoClient(connectionString);
        }
        public MongoDbContext(MongoClientSettings mongoClientSettings)
        {
            Client = new MongoClient(mongoClientSettings);
        }

        private MongoClient Client { get; set; }
        private IMongoDatabase DataBase
        {
            get
            {
                string dbName = DataBaseAttribute.GetName(typeof(TDataBase));
                if (string.IsNullOrEmpty(dbName))
                {
                    dbName = typeof(TDataBase).Name;
                }
                return Client.GetDatabase(dbName);
            }
        }
        private IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : class
        {
            string collectionName = TableAttribute.GetName(typeof(TEntity));
            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = typeof(TEntity).Name;
            }
            return DataBase.GetCollection<TEntity>(collectionName);
        }
        private IMongoCollection<BsonDocument> GetCollectionBson<TEntity>() where TEntity : class
        {
            string collectionName = TableAttribute.GetName(typeof(TEntity));
            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = typeof(TEntity).Name;
            }
            return DataBase.GetCollection<BsonDocument>(collectionName);
        }

        public void Dispose()
        {

        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            GetCollection<TEntity>().InsertOne(entity);
        }
        public void AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            GetCollection<TEntity>().InsertOneAsync(entity);
        }
        public void Add<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            GetCollection<TEntity>().InsertMany(entities);
        }
        public void AddAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            GetCollection<TEntity>().InsertManyAsync(entities);
        }
        public void Add<TEntity>(BsonDocument bsonDocument) where TEntity : class
        {
            GetCollectionBson<TEntity>().InsertOne(bsonDocument);
        }
        public void AddAsync<TEntity>(BsonDocument bsonDocument) where TEntity : class
        {
            GetCollectionBson<TEntity>().InsertOneAsync(bsonDocument);
        }
        public void Add<TEntity>(IEnumerable<BsonDocument> bsonDocuments) where TEntity : class
        {
            GetCollectionBson<TEntity>().InsertMany(bsonDocuments);
        }
        public void AddAsync<TEntity>(IEnumerable<BsonDocument> bsonDocuments) where TEntity : class
        {
            GetCollectionBson<TEntity>().InsertManyAsync(bsonDocuments);
        }

        public void Update<TEntity>(Expression<Func<TEntity, bool>> filter, TEntity entity) where TEntity : class
        {
            GetCollection<TEntity>().ReplaceOne(filter, entity);
        }
        public void UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> filter, TEntity entity) where TEntity : class
        {
            GetCollection<TEntity>().ReplaceOneAsync(filter, entity);
        }
        public void Update<TEntity>(FilterDefinition<BsonDocument> filter, BsonDocument replacement) where TEntity : class
        {
            GetCollectionBson<TEntity>().ReplaceOne(filter, replacement);
        }
        public void UpdateAsync<TEntity>(FilterDefinition<BsonDocument> filter, BsonDocument replacement) where TEntity : class
        {
            GetCollectionBson<TEntity>().ReplaceOneAsync(filter, replacement);
        }

        public void DeleteOne<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            GetCollection<TEntity>().DeleteOne(filter);
        }
        public void Delete<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            GetCollection<TEntity>().DeleteMany(filter);
        }
        public void DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            GetCollection<TEntity>().DeleteManyAsync(filter);
        }
        public void DeleteOne<TEntity>(FilterDefinition<BsonDocument> filter) where TEntity : class
        {
            GetCollectionBson<TEntity>().DeleteOne(filter);
        }
        public void Delete<TEntity>(FilterDefinition<BsonDocument> filter) where TEntity : class
        {
            GetCollectionBson<TEntity>().DeleteMany(filter);
        }
        public void DeleteAsync<TEntity>(FilterDefinition<BsonDocument> filter) where TEntity : class
        {
            GetCollectionBson<TEntity>().DeleteManyAsync(filter);
        }

        public TEntity QueryOne<TEntity>(string _id) where TEntity : class
        {
            FilterDefinitionBuilder<TEntity> builderFilter = Builders<TEntity>.Filter;
            FilterDefinition<TEntity> filter = builderFilter.Eq("_id", _id);
            return GetCollection<TEntity>().Find(filter).FirstOrDefault();
        }
        public TEntity QueryOne<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return QueryList(filter).FirstOrDefault();
        }
        public BsonDocument QueryOneBson<TEntity>(string _id) where TEntity : class
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", _id);
            return GetCollectionBson<TEntity>().Find(filter).FirstOrDefault();
        }
        public BsonDocument QueryOneBson<TEntity>(FilterDefinition<BsonDocument> filter) where TEntity : class
        {
            return GetCollectionBson<TEntity>().Find(filter).FirstOrDefault();
        }

        public List<TEntity> QueryList<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return GetCollection<TEntity>().Find(filter).ToList();
        }
        public List<BsonDocument> QueryListBson<TEntity>() where TEntity : class
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Where(t => true);
            return GetCollectionBson<TEntity>().Find(filter).ToList();
        }
        public List<BsonDocument> QueryListBson<TEntity>(FilterDefinition<BsonDocument> filter) where TEntity : class
        {
            return GetCollectionBson<TEntity>().Find(filter).ToList();
        }

        public int QueryCount<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return Convert.ToInt32(GetCollection<TEntity>().Count(filter));
        }
        public bool QueryExist<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return QueryCount<TEntity>(filter) > 0;
        }
        public int QueryCount<TEntity>(FilterDefinition<BsonDocument> filter) where TEntity : class
        {
            return Convert.ToInt32(GetCollectionBson<TEntity>().Count(filter));
        }
        public bool QueryExist<TEntity>(FilterDefinition<BsonDocument> filter) where TEntity : class
        {
            return QueryCount<TEntity>(filter) > 0;
        }
    }
}
