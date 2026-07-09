using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.Application.Commons.PaginatedResults
{
    public class PaginatedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
        public int? NextPageNumber => HasNextPage ? PageNumber + 1 : (int?)null;
        public int? PreviousPageNumber => HasPreviousPage ? PageNumber - 1 : (int?)null;
        public PaginatedResult(
            IReadOnlyList<T> items,
            int pageNumber,
            int pageSize,
            int totalCount)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }


}
