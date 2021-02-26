using System;
using System.Threading.Tasks;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Core.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        public Task<bool> AnyByAccountAsync(string accountId, string password);

        public Task<bool> AnyByIdAsync(string accountId);

        public Task<bool> AnyByEmailAsync(string email);

        public Task<bool> AnyByPhoneAsync(string phone);

        public Task AssignAccountAsync(Account account);

        public Task<Account> BanAccountAsync(Account account);

        public Task<Account> CreateAccountAsync(Account account);

        public Task<Account> GetAccountByIdAsync(string accountId);

        public Task<ItemList<Account>> GetAccountsAsync(string keyword, bool? gender, bool? isActive, DateTime? startDate, DateTime? endDate, int page, int pageSize);

        public Task<Account> UnbanAccountAsync(Account account);

        public Task UpdateAccountAsync(Account account);
    }
}