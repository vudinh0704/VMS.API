using System;
using System.ComponentModel.DataAnnotations;
using VMS.Core.Enums;

namespace VMS.Core.Entities
{
    public class Campaign
    {
        [Key]
        public int CampaignId { get; set; }

        public string AccountId { get; set; }

        public int CategoryId { get; set; }

        public int WardId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public int Participation { get; set; }

        public string Location { get; set; }

        public double MoneyBalance { get; set; }

        public int ScoreBalance { get; set; }

        public CampaignType Type { get; set; }

        public int Status { get; set; } = 0; // 0 - disapproved, 1 - approved

        public bool IsDeleted { get; set; } = false;

        public string SearchContent { get; set; }

        public DateTime RegistrationStartDate { get; set; }

        public DateTime RegistrationEndDate { get; set; }

        public DateTime ExecutionStartDate { get; set; }

        public DateTime ExecutionEndDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}