using System;
using VMS.Core.Entities;

namespace VMS.Api.Models.Functions
{
    public class FunctionDTO
    {
        public int FunctionId { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime LatestUpdate { get; set; }

        public string UpdatedBy { get; set; }

        public static FunctionDTO GetFrom(Function function)
        {
            return new FunctionDTO
            {
                FunctionId = function.FunctionId,
                Code = function.Code,
                Description = function.Description,
                IsActive = function.IsActive,
                CreatedDate = function.CreatedDate,
                CreatedBy = function.CreatedBy,
                LatestUpdate = function.UpdatedDate,
                UpdatedBy = function.UpdatedBy
            };
        }
    }
}