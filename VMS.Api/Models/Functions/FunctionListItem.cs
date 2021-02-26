using VMS.Core.Entities;

namespace VMS.Api.Models.Functions
{
    public class FunctionListItem
    {
        public int FunctionId { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public static FunctionListItem GetFrom(Function function)
        {
            return new FunctionListItem
            {
                FunctionId = function.FunctionId,
                Code = function.Code,
                Description = function.Description,
                IsActive = function.IsActive
            };
        }
    }
}