using System.Collections.Generic;

namespace web.Models
{
    public class TransactionProfile
    {
        public TransactionProfile( List<TransactionModel> transactionModelList)
        {
            this.TransactionModelList = transactionModelList;
        }

        public TransactionProfile()
        {
            TransactionModelList = new List<TransactionModel>();
            
        }

        public List<TransactionModel> TransactionModelList { get; set; }
    }
}