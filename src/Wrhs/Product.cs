using System;

namespace Wrhs
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        
        public string Code { get; set; }

        public string Name { get; set; }

        public string EAN { get; set; }

        public string SKU { get; set; }

        public string Description { get; set; }

    }
}