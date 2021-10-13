using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Solution.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Solution.Contracts.Requests.Queries;
using Solution.WebAPI.Mapping;
using Solution.DAL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace Solution.WebAPI.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Export file and save to Database
        /// </summary>
        /// <param name="file"></param>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpPost("upload")]
        public async Task<IActionResult> ExportFile(IFormFile file)
        {
            if (file == null)
                return NotFound();

            var res = await _transactionService.UploadFileSaveToDataBaseAsync(file);

            return Ok($"Status of uploading: {res}");
        }

        /// <summary>
        /// Get Transactions by Status filter
        /// </summary>
        /// <param name="query"></param>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("filter")]
        public IActionResult GetAllByFilter([FromQuery] GetAllDataQuery query)
        {
            var filter = query.DomainToRequestFilter();

            var res = _transactionService.GetAllByFilter(filter).ToList();

            return Ok(res);
        }

        /// <summary>
        /// Delete By Transaction Id
        /// </summary>
        /// <param name="modelId"></param>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("records/{modelId}")]
        public async Task<IActionResult> DeleteByIdAsync(int modelId)
        {
            if (modelId < 0 || modelId > 100_000)
                return NotFound();

            var res = await _transactionService.DeleteByIdAsync(modelId);

            return Ok($"Status of deleting: {res}");
        }

        /// <summary>
        /// Update Status By Transaction Id
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="status"></param>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpPut("records/{modelId}")]
        public async Task<IActionResult> UpdateStatusByIdAsync(int modelId, Status status)
        {
            if (modelId < 0 || modelId > 100_000)
                return NotFound();

            var res = await _transactionService.UpdateStatusByIdAsync(modelId, status);

            return Ok($"Status of updaining: {res}");
        }

        /// <summary>
        /// Find Transaction By Client Name
        /// </summary>
        /// <param name="clientName"></param>
        /// <response code="400">Unable to create a transaction due to validation error</response>
        /// <response code="401">Unauthorized</response>
        [Authorize(Roles = "Admin,User")]
        [HttpGet("records/{clientName}")]
        public IActionResult FindTransactionByClientName(string clientName)
        {
            var res = _transactionService.FindTransactionByClientName(clientName).ToList();

            return Ok(res);
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
            var res = await _transactionService.DownloadAsync();

            return File(Encoding.UTF8.GetBytes(res.ToString()), "text/csv", "TransactionInfos.csv");
        }       
    }
}