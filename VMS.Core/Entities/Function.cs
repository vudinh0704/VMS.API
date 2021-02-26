using System;
using System.ComponentModel.DataAnnotations;

namespace VMS.Core.Entities
{
    public class Function
    {
        [Key]
        public int FunctionId { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}