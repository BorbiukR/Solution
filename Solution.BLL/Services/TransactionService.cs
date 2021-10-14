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
using Solution.BLL.DTOs;

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

        public async Task<bool> DeleteByIdAsync(int transactionId)
        {
            // TODO: chech in DB if exist
            if (transactionId < 0 || transactionId > 100_000)
                throw new TransactionException("No Transaction with such Id");

            var transaction = await _context.TransactionInfos.FindAsync(transactionId);

            _context.TransactionInfos.Remove(transaction);

            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<StringBuilder> DownloadAsync()
        {
            var transactions = await _context.TransactionInfos.Select(x => x).ToListAsync();

            var builder = new StringBuilder();

            builder.AppendLine("Id,Status,Type,ClientName,Amount");

            foreach (var transaction in transactions)
            {
                builder.AppendLine(
                    $"{transaction.Id}," +
                    $"{transaction.Status}," +
                    $"{transaction.Type}," +
                    $"{transaction.ClientName}," +
                    $"{transaction.Amount}"
                    );
            }

            return builder;
        }

        public async Task<IEnumerable<TransactionInfoDTO>> GetAllByFilter(
            GetAllByFilterDTO filter = null, 
            PaginationFilterDTO paginationFilter = null)
        {
            var queryable = _context.TransactionInfos.AsQueryable();

            queryable = AddFiltersOnQuery(filter, queryable);

            if (paginationFilter == null)
            {
                return await queryable.Select(x => x.ToDto()).ToListAsync();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

            return await queryable.Select(x => x.ToDto())
                          .Skip(skip)
                          .Take(paginationFilter.PageSize)
                          .ToListAsync();
        }

        public async Task<IEnumerable<TransactionInfoDTO>> FindTransactionByClientName(string clientName)
        {
            if (string.IsNullOrEmpty(clientName))
                throw new TransactionException("Client Name Is Null Or Empty");

            var transactions = await _context.TransactionInfos
                .Where(x => x.ClientName == clientName)
                .ToListAsync();

            var maapedTransactions = transactions.Select(x => x.ToDto());

            return maapedTransactions;
        }

        public async Task<bool> UpdateStatusByIdAsync(int transactionId, Status status)
        {
            if (transactionId < 0 || transactionId > 100_000)
                throw new TransactionException("No such Transaction");

            var transaction = await _context.TransactionInfos.FindAsync(transactionId);

            transaction.Status = status;

            _context.TransactionInfos.Update(transaction);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UploadFileAndSaveToDataBaseAsync(IFormFile file)
        {
            if (file == null)
                throw new TransactionException("Error");

            string path = $"{_webHostEnvironment.WebRootPath}\\Files\\{file.FileName}";

            using (FileStream fileStream = File.Create(path))
            {
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            var transactions = GetTransactionInfoList(path);

            var mappedTransactions = Mapping(transactions);

            var exsistingRecords = CheckIfExsistData(mappedTransactions);

            _context.TransactionInfos.UpdateRange(exsistingRecords);
            _context.TransactionInfos.AddRange(mappedTransactions.Except(exsistingRecords));

            return await _context.SaveChangesAsync() > 0;
        }

        private List<TransactionInfoDTO> GetTransactionInfoList(string path)
        {
            var transactions = new List<TransactionInfoDTO>();

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

                    transactions.Add(transactionInfoDTO);
                }
            }

            return transactions;
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