using Solution.Contracts.Requests.Queries;
using System;

namespace Solution.BLL.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationQuery pagination = null);
    }
}