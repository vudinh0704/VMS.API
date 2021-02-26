using System;

namespace VMS.Api.Models.Accounts
{
    public class AccountGetModel : BaseGetModelWithPagination
    {
        public string Keyword { get; set; }

        public bool? Gender { get; set; }        

        public bool? IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}