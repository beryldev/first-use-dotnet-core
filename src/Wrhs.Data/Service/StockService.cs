using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Data.Service
{
    public class StockService : BaseData, IStockService
    {
        public StockService(WrhsContext context) : base(context)
        {
        }

        public IEnumerable<Stock> GetProductStock(int productId)
        {
            var items = context.Shifts
                .Where(s => s.ProductId == productId && s.Confirmed)
                .GroupBy(s => s.Location)
                .Select(s => new Stock
                {
                    ProductId = productId,
                    Location = s.First().Location,
                    Quantity = s.Sum(x=>x.Quantity)
                }).ToList();

            return items;
        }

        public Stock GetStockAtLocation(int productId, string location)
        {
            var sum = context.Shifts
                .Where(s => s.Confirmed && s.ProductId == productId && s.Location == location)
                .Sum(s => s.Quantity);

            return new Stock
            {
                ProductId = productId,
                Location = location,
                Quantity = sum,
                Product = context.Products.Find(productId)
            };
        }

        public ResultPage<Stock> GetStocks(int page, int pageSize)
        {
            var pg = page < 1 ? 0 : page-1;

            var result =  context.Shifts
                .Where(s => s.Confirmed)
                .Include(s=>s.Product)
                .GroupBy(s => new {a=s.ProductId, b=s.Location})
                .Select(x => new Stock
                {
                    ProductId = x.First().ProductId,
                    Location = x.First().Location,
                    Quantity = x.Sum(y=>y.Quantity),
                    Product = x.First().Product
                })          
                .Skip(pg * pageSize)
                .Take(pageSize)          
                .ToList();

            return new ResultPage<Stock>(result, page, pageSize);    
        }
    }
}