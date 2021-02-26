using System;
using VMS.Core.Entities;

namespace VMS.Api.Models.Accounts
{
    public class AccountDTO
    {
        public string AccountId { get; set; }

        public int? GroupId { get; set; }

        public int? WardId { get; set; }

        public string Name { get; set; }

        public bool? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

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

        public static AccountDTO GetFrom(Account account)
        {
            return new AccountDTO
            {
                AccountId = account.AccountId,
                WardId = account.WardId,
                GroupId = account.GroupId,
                Name = account.Name,
                Gender = account.Gender,
                BirthDate = account.BirthDate,
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