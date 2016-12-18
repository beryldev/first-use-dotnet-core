using System;
using Wrhs.Core;

namespace Wrhs.Products
{
    public class DeleteProductEvent : IEvent
    {
        public DeleteProductEvent(Product product, DateTime deletedAt)
        {
            this.Product = product;
            this.DeletedAt = deletedAt;

        }
        public Product Product { get; }

        public DateTime DeletedAt { get; }
    }
}