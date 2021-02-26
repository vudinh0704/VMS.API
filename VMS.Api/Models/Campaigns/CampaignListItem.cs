using System;
using VMS.Core.Entities;
using VMS.Core.Enums;

namespace VMS.Api.Models.Campaigns
{
    public class CampaignListItem
    {
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

        public DateTime RegistrationStartDate { get; set; }

        public DateTime RegistrationEndDate { get; set; }

        public DateTime ExecutionStartDate { get; set; }

        public DateTime ExecutionEndDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LatestUpdate { get; set; }

        public static CampaignListItem GetFrom(Campaign campaign)
        {
            return new CampaignListItem
            {
                CampaignId = campaign.CampaignId,
                AccountId = campaign.AccountId,
                CategoryId = campaign.CategoryId,
                WardId = campaign.WardId,
                Name = campaign.Name,
                Image = campaign.Image,
                Description = campaign.Description,
                Participation = campaign.Participation,
                Location = campaign.Location,
                MoneyBalance = campaign.MoneyBalance,
                ScoreBalance = campaign.ScoreBalance,
                Type = campaign.Type,
                RegistrationStartDate = campaign.RegistrationStartDate,
                RegistrationEndDate = campaign.RegistrationEndDate,
                ExecutionStartDate = campaign.ExecutionStartDate,
                ExecutionEndDate = campaign.ExecutionEndDate,
                CreatedDate = campaign.CreatedDate,
                LatestUpdate = campaign.UpdatedDate
            };
        }
    }
}