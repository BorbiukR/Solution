using Solution.BLL.DTOs;
using Solution.Contracts.Requests.Queries;

namespace Solution.WebAPI.Mapping
{
    public static class PaginationRequestToDTO
    {
        public static PaginationFilterDTO DomainToRequest(this PaginationQuery model)
        {
            if (model != null)
            {
                return new PaginationFilterDTO
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                };
            }

            return null;
        }
    }
}