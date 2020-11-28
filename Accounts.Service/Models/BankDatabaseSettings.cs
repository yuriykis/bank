namespace Accounts.Service.Models
{

    public class BankDatabaseSettings : IBankDatabaseSettings
    {
        public string AccountsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IBankDatabaseSettings
    {
        string AccountsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
