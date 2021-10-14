using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Solution.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Solution.Contracts.Requests.Queries;
using Solution.WebAPI.Mapping;
using Solution.DAL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Solution.WebAPI.Helpers;
using Solution.Contracts.Responses.Pagination;
using Solution.BLL.DTO;
using System.Linq;

namespace Solution.WebAPI.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IUriService _uriService;

        public TransactionController(
            ITransactionService transactionService, 
            IUriService uriService)
        {
            _transactionService = transactionService;
            _uriService = uriService;
        }

        /// <summary>
        /// Upload file and save to Database
        /// </summary>
        /// <param name="file"></param>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null)
                return NotFound();

            var result = await _transactionService.UploadFileAndSaveToDataBaseAsync(file);

            return Ok($"Status of uploading: {result}");
        }

        /// <summary>
        /// Get Transactions by Status filter
        /// </summary>
        /// <param name="query"></param>
        /// <param name="paginationQuery"></param>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("filter")]
        public async Task<IActionResult> GetAllByFilter(
            [FromQuery] GetAllDataQuery query, 
            [FromQuery] PaginationQuery paginationQuery)
        {
            var filter = query.DomainToRequestFilter();
            var pagination = paginationQuery.DomainToRequest();
            var transactions = await _transactionService.GetAllByFilter(filter, pagination);

            var cardFilesPaginationResponse = new PagedResponse<TransactionInfoDTO>(transactions);

            if (transactions == null || cardFilesPaginationResponse == null)
                return NotFound();

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
                return Ok(cardFilesPaginationResponse);

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, transactions.ToList());

            return Ok(paginationResponse);
        }

        /// <summary>
        /// Delete By Transaction Id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("records/{modelId}")]
        public async Task<IActionResult> DeleteByIdAsync(int transactionId)
        {
            if (transactionId < 0 || transactionId > 100_000)
                return NotFound();

            var result = await _transactionService.DeleteByIdAsync(transactionId);

            return Ok($"Status of deleting: {result}");
        }

        /// <summary>
        /// Update Status By Transaction Id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="status"></param>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpPut("records/{modelId}")]
        public async Task<IActionResult> UpdateStatusByIdAsync(int transactionId, Status status)
        {
            if (transactionId < 0 || transactionId > 100_000)
                return NotFound();

            var result = await _transactionService.UpdateStatusByIdAsync(transactionId, status);

            return Ok($"Status of updaining: {result}");
        }

        /// <summary>
        /// Find Transaction By Client Name
        /// </summary>
        /// <param name="clientName"></param>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("records/{clientName}")]
        public async Task<IActionResult> FindTransactionByClientName(string clientName)
        {
            var transactions = await _transactionService.FindTransactionByClientName(clientName);

            return Ok(transactions);
        }

        /// <summary>
        /// Download all Transactions into csv file
        /// </summary>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("download")]
        public async Task<IActionResult> DownloadAsync()
        {
            var result = await _transactionService.DownloadAsync();

            return File(Encoding.UTF8.GetBytes(result.ToString()), "text/csv", "TransactionInfos.csv");
        }       
    }
}