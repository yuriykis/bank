using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Transactions.Service.Models;
using Transactions.Service.Persistence;

namespace Transactions.Service.Services
{
    public class TransactionService
    {
        private readonly PrimaryContext _context;
        public TransactionService(PrimaryContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> Get()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction> Get(string id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<List<Transaction>> GetBySenderId(string id)
        {
            var response = await _context.Transactions.Where(transaction => transaction.SenderAccountId == id).ToListAsync();
            return response;
        }
        
        public async Task<List<Transaction>> GetByReceiverId(string id)
        {
            return await _context.Transactions.Where(transaction => transaction.ReceiverAccountId == id).ToListAsync();
        }
        public async Task<Transaction> Create(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task Update(string id, Transaction transactionIn)
        {
            _context.Transactions.Update(transactionIn);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Transaction transactionIn)
        {
            _context.Transactions.Remove(transactionIn);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(string id)
        {
            var transaction = await  _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
