using api.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class FilmService
    {
        private readonly IMongoCollection<Film> _films;

        public FilmService(ICinemaDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _films = database.GetCollection<Film>(settings.FilmsCollectionName);
        }

        public List<Film> Get() =>
            _films.Find(film => true).ToList();

        public Film Get(string id) =>
            _films.Find<Film>(film => film.id == id).FirstOrDefault();

        public Film Create(Film film)
        {
            _films.InsertOne(film);
            return film;
        }

        public void Update(string id, Film filmIn) =>
            _films.ReplaceOne(film => film.id == id, filmIn);

        public void Remove(Film filmIn) =>
            _films.DeleteOne(film => film.id == filmIn.id);

        public void Remove(string id) =>
            _films.DeleteOne(film => film.id == id);
    }
}
