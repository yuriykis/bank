using api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class RoomService
    {
        private readonly IMongoCollection<Room> _rooms;

        public RoomService(ICinemaDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _rooms = database.GetCollection<Room>(settings.RoomsCollectionName);
        }

        public List<Room> Get() =>
            _rooms.Find(room => true).ToList();

        public Room Get(string id) =>
            _rooms.Find<Room>(room => room.id == id).FirstOrDefault();

        public Room Create(Room room)
        {
            _rooms.InsertOne(room);
            return room;
        }

        public void Update(string id, Room roomIn) =>
            _rooms.ReplaceOne(room => room.id == id, roomIn);

        public void Remove(Room roomIn) =>
            _rooms.DeleteOne(room => room.id == roomIn.id);

        public void Remove(string id) =>
            _rooms.DeleteOne(room => room.id == id);
    }
}
