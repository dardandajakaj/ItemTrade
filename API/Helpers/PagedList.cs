using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public PagedList(IEnumerable<T> items, int totalItems, int itemsPerPage, int currentPage)
        {
            TotalItems = totalItems;
            ItemsPerPage = itemsPerPage;
            TotalPages = (int) Math.Ceiling(totalItems/(double)itemsPerPage);
            CurrentPage = currentPage;
            AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int pageNumber, int pageSize){
            var count = await query.CountAsync();
            var result = await query.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(result,count,pageSize,pageNumber);
        }
    }
}