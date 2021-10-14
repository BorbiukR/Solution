using System;
using System.Collections.Generic;

namespace Solution.Contracts.Responses.Pagination
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }

        public PagedResponse() { }

        public PagedResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
    }
}