using api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class WatcherService
    {
        private readonly IMongoCollection<Watcher> _watchers;

        public WatcherService(ICinemaDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _watchers = database.GetCollection<Watcher>(settings.WatchersCollectionName);
        }

        public List<Watcher> Get() =>
            _watchers.Find(watcher => true).ToList();

        public Watcher Get(string id) =>
            _watchers.Find<Watcher>(watcher => watcher.id == id).FirstOrDefault();

        public Watcher Create(Watcher watcher)
        {
            _watchers.InsertOne(watcher);
            return watcher;
        }

        public void Update(string id, Watcher watcherIn) =>
            _watchers.ReplaceOne(watcher => watcher.id == id, watcherIn);

        public void Remove(Watcher watcherIn) =>
            _watchers.DeleteOne(watcher => watcher.id == watcherIn.id);

        public void Remove(string id) =>
            _watchers.DeleteOne(watcher => watcher.id == id);
    }
}
