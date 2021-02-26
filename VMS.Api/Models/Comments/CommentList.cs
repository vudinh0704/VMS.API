using System.Linq;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Api.Models.Comments
{
    public class CommentList : ItemList<CommentListItem>
    {
        public static CommentList GetFrom(ItemList<Comment> list)
        {
            return new CommentList
            {
                Total = list.Total,
                Page = list.Page,
                PageSize = list.PageSize,
                Items = list.Items.Select(item => CommentListItem.GetFrom(item)).ToList()
            };
        }
    }
}