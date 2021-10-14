using Microsoft.AspNetCore.Http;
using Solution.BLL.DTO;
using Solution.BLL.DTOs;
using Solution.DAL.Enum;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Solution.BLL.Interfaces
{
    public interface ITransactionService : ICrud<TransactionInfoDTO>
    {
        Task<bool> UpdateStatusByIdAsync(int transactionId, Status status);

        Task<IEnumerable<TransactionInfoDTO>> FindTransactionByClientName(string clientName);

        Task<bool> UploadFileAndSaveToDataBaseAsync(IFormFile file);

        Task<IEnumerable<TransactionInfoDTO>> GetAllByFilter(
            GetAllByFilterDTO filter = null, 
            PaginationFilterDTO paginationFilter = null); 

        Task<StringBuilder> DownloadAsync();      
    }
}