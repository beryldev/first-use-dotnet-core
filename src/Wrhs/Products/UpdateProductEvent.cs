using System;
using Wrhs.Core;

namespace Wrhs.Products
{
    public class UpdateProductEvent : IEvent
    {
        public UpdateProductEvent(Product product, DateTime updatedAt)
        {
            this.Product = product;
            this.UpdatedAt = updatedAt;

        }
        public Product Product { get; }

        public DateTime UpdatedAt { get; }
    }
}