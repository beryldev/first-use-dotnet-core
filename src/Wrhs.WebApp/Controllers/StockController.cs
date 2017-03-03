using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;

namespace Wrhs.WebApp.Controllers
{
    [Route("api/stock")]
    public class StockController : BaseController
    {
        private readonly IStockService stockSrv;

        public StockController(IStockService stockSrv)
        {
            this.stockSrv = stockSrv;
        }

        [HttpGet]
        public IActionResult GetStocks(int page=1, int pageSize=20)
        {
            var result = stockSrv.GetStocks(page, pageSize);
            return Ok(result);
        }
    }
}