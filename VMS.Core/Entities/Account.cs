using System;
using System.ComponentModel.DataAnnotations;

namespace VMS.Core.Entities
{
    public class Account
    {
        [Key]
        public string AccountId { get; set; }

        public int WardId { get; set; }

        public int GroupId { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public bool? Gender { get; set; } = null;

        public DateTime? BirthDate { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Avatar { get; set; }

        public string Description { get; set; }

        public int Reliability { get; set; }

        public double MoneyBalance { get; set; }

        public int ScoreBalance { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public string SearchContent { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }        
    }
}