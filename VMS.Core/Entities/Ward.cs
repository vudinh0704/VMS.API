using System.ComponentModel.DataAnnotations;

namespace VMS.Core.Entities
{
    public class Ward
    {
        [Key]
        public int WardId { get; set; }

        public int DistrictId { get; set; }

        public int CityId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}