using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{

    public class CinemaDatabaseSettings : ICinemaDatabaseSettings
    {
        public string RoomsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string FilmsCollectionName { get; set; }
        public string FilmShowingsCollectionName { get; set; }
        public string WatchersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ICinemaDatabaseSettings
    {
        string RoomsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string FilmsCollectionName { get; set; }
        string FilmShowingsCollectionName { get; set; }
        string WatchersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
