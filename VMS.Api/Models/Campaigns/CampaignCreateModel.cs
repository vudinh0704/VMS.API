using System;
using VMS.Core.Enums;

namespace VMS.Api.Models.Campaigns
{
    public class CampaignCreateModel
    {
        public int CategoryId { get; set; }

        public int WardId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public CampaignType Type { get; set; }

        public DateTime RegistrationStartDate { get; set; } = DateTime.MinValue;

        public DateTime RegistrationEndDate { get; set; } = DateTime.MinValue;

        public DateTime ExecutionStartDate { get; set; } = DateTime.MinValue;

        public DateTime ExecutionEndDate { get; set; } = DateTime.MinValue;
    }
}