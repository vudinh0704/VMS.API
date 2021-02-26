using System.Linq;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Api.Models.Campaigns
{
    public class CampaignList : ItemList<CampaignListItem>
    {
        public static CampaignList GetFrom(ItemList<Campaign> list)
        {
            return new CampaignList
            {
                Total = list.Total,
                Page = list.Page,
                PageSize = list.PageSize,
                Items = list.Items.Select(item => CampaignListItem.GetFrom(item)).ToList()
            };
        }
    }
}