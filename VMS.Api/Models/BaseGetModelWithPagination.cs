using VMS.Api.Exceptions;

namespace VMS.Api.Models
{
    public abstract class BaseGetModelWithPagination
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;

        public virtual void Validate()
        {
            if (Page < 1)
            {
                throw new PageIsInvalidException();
            }

            if (PageSize < 1)
            {
                throw new PageSizeIsInvalidException();
            }
        }
    }
}