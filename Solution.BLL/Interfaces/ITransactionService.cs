using Microsoft.AspNetCore.Http;
using Solution.BLL.DTO;
using Solution.DAL.Enum;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Solution.BLL.Interfaces
{
    public interface ITransactionService : ICrud<TransactionInfoDTO>
    {
        Task<bool> UpdateStatusByIdAsync(int modelId, Status status);

        IEnumerable<TransactionInfoDTO> FindTransactionByClientName(string clientName);

        Task<bool> UploadFileSaveToDataBaseAsync(IFormFile file); 

        IEnumerable<TransactionInfoDTO> GetAllByFilter(GetAllByFilterDTO filter = null); 

        Task<StringBuilder> DownloadAsync();      
    }
}