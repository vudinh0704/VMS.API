using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VMS.Core.Entities;
using VMS.Core.Helpers;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Infrastructure.Data
{
    public class AccountRepository : BaseRepository<AccountRepository>, IAccountRepository
    {
        private readonly VMSDbContext _dbContext;

        public AccountRepository(VMSDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<bool> AnyByAccountAsync(string accountId, string password)
        {
            return await _dbContext.Accounts.AnyAsync(item => item.AccountId.Equals(accountId) && item.Password.Equals(password));
        }

        public async Task<bool> AnyByEmailAsync(string email)
        {
            return await _dbContext.Accounts.AnyAsync(item => item.Email.Equals(email));
        }

        public async Task<bool> AnyByIdAsync(string accountId)
        {
            return await _dbContext.Accounts.AnyAsync(item => item.AccountId.Equals(accountId));
        }

        public async Task<bool> AnyByPhoneAsync(string phone)
        {
            return await _dbContext.Accounts.AnyAsync(item => item.Phone.Equals(phone));
        }

        public async Task AssignAccountAsync(Account account)
        {
            _dbContext.Entry(account).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Account> BanAccountAsync(Account account)
        {
            _dbContext.Entry(account).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<Account> GetAccountByIdAsync(string accountId)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(item => item.AccountId.Equals(accountId));
        }

        public async Task<ItemList<Account>> GetAccountsAsync(string keyword, bool? gender, bool? isActive, DateTime? startDate, DateTime? endDate, int page, int pageSize)
        {
            var result = new ItemList<Account> { Page = page, PageSize = pageSize };
            var accounts = _dbContext.Accounts.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                accounts = accounts.Where(item => item.SearchContent.Contains(keyword));
            }

            if (gender.HasValue)
            {
                accounts = accounts.Where(item => item.Gender == gender.Value);
            }

            if (isActive.HasValue)
            {
                accounts = accounts.Where(item => item.IsActive == isActive.Value);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                endDate = endDate.Value.AddDays(1);

                accounts = accounts.Where(item => item.CreatedDate >= startDate.Value && item.CreatedDate < endDate.Value);
            }

            result.Total = await accounts.CountAsync();
            result.Items = await GetList(accounts, page, pageSize);

            return result;
        }

        public async Task<Account> UnbanAccountAsync(Account account)
        {
            _dbContext.Entry(account).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return account;
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _dbContext.Entry(account).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}