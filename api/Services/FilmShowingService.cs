using api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class FilmShowingService
    {
        private readonly IMongoCollection<FilmShowing> _filmShowings;

        public FilmShowingService(ICinemaDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _filmShowings = database.GetCollection<FilmShowing>(settings.FilmShowingsCollectionName);
        }

        public List<FilmShowing> Get() =>
            _filmShowings.Find(film => true).ToList();

        public FilmShowing Get(string id) =>
            _filmShowings.Find<FilmShowing>(filmShowing => filmShowing.id == id).FirstOrDefault();

        public FilmShowing Create(FilmShowing filmShowing)
        {
            _filmShowings.InsertOne(filmShowing);
            return filmShowing;
        }

        public void Update(string id, FilmShowing filmShowingIn) =>
            _filmShowings.ReplaceOne(filmShowings => filmShowings.id == id, filmShowingIn);

        public void Remove(FilmShowing filmShowingIn) =>
            _filmShowings.DeleteOne(filmShowing => filmShowing.id == filmShowingIn.id);

        public void Remove(string id) =>
            _filmShowings.DeleteOne(filmShowing => filmShowing.id == id);
    }
}
