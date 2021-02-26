using System.Linq;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Api.Models.Functions
{
    public class FunctionList : ItemList<FunctionListItem>
    {
        public static FunctionList GetFrom(ItemList<Function> list)
        {
            return new FunctionList
            {
                Total = list.Total,
                Page = list.Page,
                PageSize = list.PageSize,
                Items = list.Items.Select(item => FunctionListItem.GetFrom(item)).ToList()
            };
        }
    }
}