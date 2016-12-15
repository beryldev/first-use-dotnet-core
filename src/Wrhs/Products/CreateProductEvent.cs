using System;
using Wrhs.Core;

namespace Wrhs.Products
{
    public class CreateProductEvent : IEvent
    {
        public CreateProductEvent(Product product, DateTime createdAt)
        {
            this.Product = product;
            this.CreatedAt = createdAt;

        }
        public Product Product { get; }

        public DateTime CreatedAt { get; }
    }
}