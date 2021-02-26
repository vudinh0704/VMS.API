using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VMS.Core.Entities;
using VMS.Core.Enums;
using VMS.Core.Helpers;

namespace VMS.Core.Interfaces.Repositories
{
    public interface ICampaignRepository
    {
        public Task<Campaign> CreateCampaignAsync(Campaign campaign);

        public Task<Campaign> GetCampaignByIdAsync(int campaignId);

        public Task<ItemList<Campaign>> GetCampaignsAsync(string keyword, int? category, CampaignType? type, DateType dateType, DateTime? period, int page, int pageSize);

        public Task UpdateCampaignAsync(Campaign campaign);
    }
}