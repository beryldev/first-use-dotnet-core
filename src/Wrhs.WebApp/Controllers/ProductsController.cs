using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Core.Search.Interfaces;
using Wrhs.Data;
using Wrhs.Data.ContextFactory;
using Wrhs.Data.Repository;
using Wrhs.Products;
using Wrhs.Products.Commands;
using Wrhs.Products.Search;

namespace Wrhs.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        IRepository<Product> repo;
        
        public ProductsController()
        {
            var context = SqliteContextFactory.Create("Filename=./wrhs.db");
            context.Database.EnsureCreated();
            repo = new ProductRepository(context);
        }

        [HttpGet]
        public IPaginateResult<Product> Get()
        {
            var paginator = new Paginator<Product>();
            var search = new ProductSearch(repo, paginator);
            var criteria = (ProductSearchCriteria)search.MakeCriteria();
            criteria.WhereCode(Condition.Equal, "PROD1");
            return search.Exec(criteria);
        }

        [HttpPost]
        public void Post(CreateProductCommand cmd)
        {
            var handler = new CreateProductCommandHandler(repo);
            var validator = new CreateProductCommandValidator(repo);
            var service = new ValidationCommandHandlerDecorator<CreateProductCommand>
                (handler, validator);

            service.Handle(cmd);
        }
    }
}