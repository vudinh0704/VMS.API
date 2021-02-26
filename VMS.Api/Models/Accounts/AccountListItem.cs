using System;
using VMS.Core.Entities;

namespace VMS.Api.Models.Accounts
{
    public class AccountListItem
    {
        public string AccountId { get; set; }

        public int GroupId { get; set; }

        public int WardId { get; set; }

        public string Name { get; set; }

        public bool Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Avatar { get; set; }

        public int Reliability { get; set; }

        public double MoneyBalance { get; set; }

        public int ScoreBalance { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LatestUpdate { get; set; }

        public static AccountListItem GetFrom(Account account)
        {
            return new AccountListItem
            {
                AccountId = account.AccountId,
                WardId = account.WardId,
                GroupId = account.GroupId,
                Name = account.Name,
                Gender = account.Gender.Value,
                BirthDate = account.BirthDate.Value,
                Address = account.Address,
                Email = account.Email,
                Phone = account.Phone,
                Avatar = account.Avatar,
                Reliability = account.Reliability,
                MoneyBalance = account.MoneyBalance,
                ScoreBalance = account.ScoreBalance,
                IsActive = account.IsActive,
                CreatedDate = account.CreatedDate,
                LatestUpdate = account.UpdatedDate
            };
        }
    }
}