using Microsoft.AspNetCore.Mvc;

namespace Wrhs.WebApp.Controllers
{
    [Route("api/stock")]
    public class StockController : BaseController
    {
        private readonly IWarehouse warehouse;

        public StockController(IWarehouse warehouse)
        {
            this.warehouse = warehouse;
        }

        public IActionResult GetStocks()
        {
            return Ok(warehouse.ReadStocks());
        }
    }
}