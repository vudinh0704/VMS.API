using System.ComponentModel.DataAnnotations;

namespace VMS.Core.Entities
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}