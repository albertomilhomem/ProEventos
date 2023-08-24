using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Models
{
    public class PageParams
    {
        public const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public string Term { get; set; } = string.Empty;
        private int PageSize = 10;
        public int pageSize
        {
            get { return PageSize; }
            set { PageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

    }
}