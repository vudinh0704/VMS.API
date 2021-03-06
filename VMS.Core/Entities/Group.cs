﻿using System;
using System.ComponentModel.DataAnnotations;

namespace VMS.Core.Entities
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }
}