using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class PageParams
    {
        public const int MaxItemsPerPage = 20; 
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int ItemsPerPage{
            get => _pageSize;
            set => _pageSize = (value > MaxItemsPerPage)? MaxItemsPerPage : value;
        }
    }
}