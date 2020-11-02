using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{

    public class BankDatabaseSettings : IBankDatabaseSettings
    {
        public string AccountsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string TransactionsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IBankDatabaseSettings
    {
        string AccountsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string TransactionsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
