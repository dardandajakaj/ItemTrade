using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int totalItems, int itemsPerPage, int totalPages, int currentPage)
        {
            TotalItems = totalItems;
            ItemsPerPage = itemsPerPage;
            TotalPages = totalPages;
            CurrentPage = currentPage;
        }

        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

    }
}