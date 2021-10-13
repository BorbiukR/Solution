using Microsoft.AspNetCore.Http;
using Solution.BLL.DTO;
using Solution.BLL.Interfaces;
using Solution.DAL;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using CsvHelper;
using Solution.DAL.Enum;
using Solution.BLL.Mapping;
using Solution.DAL.Entities;
using System.Transactions;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Solution.BLL.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly SolulitonContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TransactionService(SolulitonContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> DeleteByIdAsync(int modelId)
        {
            if (modelId < 0 || modelId > 100_000)
                throw new TransactionException("No such Transaction");

            var model = await _context.TransactionInfos.FindAsync(modelId);

            _context.TransactionInfos.Remove(model);

            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<StringBuilder> DownloadAsync()
        {
            var res = await _context.TransactionInfos.Select(x => x).ToListAsync();

            var builder = new StringBuilder();

            builder.AppendLine("Id,Status,Type,ClientName,Amount");

            foreach (var item in res)
            {
                builder.AppendLine($"{item.Id},{item.Status},{item.Type},{item.ClientName},{item.Amount}");
            }

            return builder;
        }

        public IEnumerable<TransactionInfoDTO> GetAllByFilter(GetAllByFilterDTO filter = null)
        {
            var queryable = _context.TransactionInfos.AsQueryable();

            queryable = AddFiltersOnQuery(filter, queryable);

            var res = queryable.Select(x => x.ToDto()).ToList();

            return res;
        }

        public IEnumerable<TransactionInfoDTO> FindTransactionByClientName(string clientName)
        {
            if (string.IsNullOrEmpty(clientName))
                throw new TransactionException("Client Name Is Null Or Empty");

            var list = _context.TransactionInfos.Where(x => x.ClientName == clientName).ToList();

            var maapedList = list.Select(x => x.ToDto());

            return maapedList;
        }

        public async Task<bool> UpdateStatusByIdAsync(int modelId, Status status)
        {
            if (modelId < 0 || modelId > 100_000)
                throw new TransactionException("No such Transaction");

            var model = await _context.TransactionInfos.FindAsync(modelId);

            model.Status = status;

            _context.TransactionInfos.Update(model);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UploadFileSaveToDataBaseAsync(IFormFile file)
        {
            if (file == null)
                throw new TransactionException("Error");

            string path = $"{_webHostEnvironment.WebRootPath}\\Files\\{file.FileName}";

            using (FileStream fileStream = File.Create(path))
            {
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            var list = GetTransactionInfoList(path);

            var mappedList = Mapping(list);

            var exsistingRecords = CheckIfExsistData(mappedList);

            _context.TransactionInfos.UpdateRange(exsistingRecords);
            _context.TransactionInfos.AddRange(mappedList.Except(exsistingRecords));

            return await _context.SaveChangesAsync() > 0;
        }

        private List<TransactionInfoDTO> GetTransactionInfoList(string path)
        {
            var list = new List<TransactionInfoDTO>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var transactionInfoDTO = new TransactionInfoDTO
                    {
                        TransactionId = csv.GetField<int>("TransactionId"),
                        Status = csv.GetField<Status>("Status"),
                        Type = csv.GetField<TransactionType>("Type"),
                        ClientName = csv.GetField<string>("ClientName"),
                        Amount = csv.GetField<string>("Amount"),
                    };

                    list.Add(transactionInfoDTO);
                }
            }

            return list;
        }

        private IEnumerable<TransactionInfo> Mapping(List<TransactionInfoDTO> list) =>
            list.Select(x => x.ToDomain());
        
        private IEnumerable<TransactionInfo> CheckIfExsistData(IEnumerable<TransactionInfo> mappedList) =>        
            mappedList.Where(x => _context.TransactionInfos.Any(z => z.Id == x.Id));

        private static IQueryable<TransactionInfo> AddFiltersOnQuery(
            GetAllByFilterDTO filter, 
            IQueryable<TransactionInfo> queryable)
        {
            if (filter.TransactionType != null && filter.Status != null)
                queryable = queryable.Where(x => x.Type == filter.TransactionType && x.Status == filter.Status);

            if (filter.TransactionType != null)
                queryable = queryable.Where(x => x.Type == filter.TransactionType);

            if (filter.Status != null)
                queryable = queryable.Where(x => x.Status == filter.Status);

            return queryable;
        }
    }
}