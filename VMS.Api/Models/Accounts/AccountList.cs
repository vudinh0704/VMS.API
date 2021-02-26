using System.Linq;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Api.Models.Accounts
{
    public class AccountList : ItemList<AccountListItem>
    {
        public static AccountList GetFrom(ItemList<Account> list)
        {
            return new AccountList
            {
                Total = list.Total,
                Page = list.Page,
                PageSize = list.PageSize,
                Items = list.Items.Select(item => AccountListItem.GetFrom(item)).ToList()
            };
        }
    }
}