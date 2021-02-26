using System;

namespace VMS.Api.Models.Accounts
{
    public class AccountUpdateModel
    {
        public int? WardId { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public bool? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Avatar { get; set; }

        public string Description { get; set; }
    }
}