using System.ComponentModel.DataAnnotations;

namespace VMS.Core.Entities
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        public string AccountId { get; set; }

        public int CampaignId { get; set; }

        public string FeedBack { get; set; }
    }
}