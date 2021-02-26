using System.Linq;
using VMS.Core.Entities;
using VMS.Core.Helpers;

namespace VMS.Api.Models.Posts
{
    public class PostList : ItemList<PostListItem>
    {
        public static PostList GetFrom(ItemList<Post> list)
        {
            return new PostList
            {
                Total = list.Total,
                Page = list.Page,
                PageSize = list.PageSize,
                Items = list.Items.Select(item => PostListItem.GetFrom(item)).ToList()
            };
        }
    }
}