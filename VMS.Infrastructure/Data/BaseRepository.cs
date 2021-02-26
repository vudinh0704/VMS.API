using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VMS.Infrastructure.Data
{
    public abstract class BaseRepository<T> where T : class
    {
        public int GetStartIndex(int page, int pageSize)
        {
            return (page - 1) * pageSize;
        }

        public async Task<List<TData>> GetList<TData>(IQueryable<TData> data, int page, int pageSize)
        {
            if (pageSize == int.MaxValue)
            {
                return await data.ToListAsync();
            }

            if (page != 1)
            {
                data = data.Skip(GetStartIndex(page, pageSize));
            }

            return await data.Take(pageSize).ToListAsync();
        }
    }
}