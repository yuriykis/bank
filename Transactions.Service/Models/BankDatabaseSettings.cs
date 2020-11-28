namespace Transactions.Service.Models
{

    public class BankDatabaseSettings : IBankDatabaseSettings
    {
        public string TransactionsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IBankDatabaseSettings
    {
        string TransactionsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
