using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Service.Models;
using Accounts.Service.Persistence;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Accounts.Service.Services
{
    public class AccountService
    {
        private readonly PrimaryContext _context;

        public AccountService(PrimaryContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> Get()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> Get(string id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Account> GetByUserId(string userId)
        {
            return await _context.Accounts.SingleOrDefaultAsync(
                account =>  account.UserId == userId
            );
        }
        public async Task<Account> Create(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task Update(string id, Account accountIn)
        {
            _context.Accounts.Update(accountIn);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Account accountIn)
        {
            _context.Accounts.Remove(accountIn);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(string id)
        {
            var account = await  _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}
