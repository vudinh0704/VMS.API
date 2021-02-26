using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Core.Entities;
using VMS.Core.Enums;
using VMS.Core.Helpers;
using VMS.Core.Interfaces.Repositories;

namespace VMS.Infrastructure.Data
{
    public class CampaignRepository : BaseRepository<CampaignRepository>, ICampaignRepository
    {
        private readonly VMSDbContext _dbContext;

        public CampaignRepository(VMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Campaign> CreateCampaignAsync(Campaign campaign)
        {
            await _dbContext.Campaigns.AddAsync(campaign);
            await _dbContext.SaveChangesAsync();

            return campaign;
        }

        public async Task<Campaign> GetCampaignByIdAsync(int campaignId)
        {
            return await _dbContext.Campaigns.FirstOrDefaultAsync(item => item.CampaignId.Equals(campaignId));
        }

        public async Task<ItemList<Campaign>> GetCampaignsAsync(string keyword, int? category, CampaignType? type, DateType dateType, DateTime? period, int page, int pageSize)
        {
            var result = new ItemList<Campaign> { Page = page, PageSize = pageSize };
            var campaigns = _dbContext.Campaigns.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                campaigns = campaigns.Where(item => item.SearchContent.Contains(keyword));
            }

            if (category.HasValue)
            {
                campaigns = campaigns.Where(item => item.CategoryId == category.Value);
            }

            if (type.HasValue)
            {
                campaigns = campaigns.Where(item => item.Type == type.Value);
            }

            switch (dateType)
            {
                case DateType.RegistrationDate:
                    if (period.HasValue)
                    {
                        campaigns = campaigns.Where(item => (item.RegistrationStartDate.Month == period.Value.Month && item.RegistrationStartDate.Year == period.Value.Year)
                                                            || (item.RegistrationEndDate.Month == period.Value.Month && item.RegistrationEndDate.Year == period.Value.Year));
                    }
                    break;
                case DateType.ExecutionDate:
                    if (period.HasValue)
                    {
                        campaigns = campaigns.Where(item => (item.ExecutionStartDate.Month == period.Value.Month && item.ExecutionStartDate.Year == period.Value.Year)
                                                            || (item.ExecutionEndDate.Month == period.Value.Month && item.ExecutionEndDate.Year == period.Value.Year));
                    }
                    break;
            }

            campaigns = campaigns.OrderBy(item => item.CreatedDate);

            result.Total = await campaigns.CountAsync();
            result.Items = await GetList(campaigns, page, pageSize);

            return result;
        }

        public async Task UpdateCampaignAsync(Campaign campaign)
        {
            _dbContext.Entry(campaign).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}