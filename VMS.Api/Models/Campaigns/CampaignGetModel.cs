using System;
using VMS.Core.Enums;

namespace VMS.Api.Models.Campaigns
{
    public class CampaignGetModel : BaseGetModelWithPagination
    {
        public string Keyword { get; set; }

        public int? Category { get; set; }

        public CampaignType? Type { get; set; }

        public DateType DateType { get; set; }

        public DateTime? Period { get; set; }
    }
}