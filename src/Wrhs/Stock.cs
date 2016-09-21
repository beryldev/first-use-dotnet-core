using System;

namespace Wrhs
{
    public class Stock
    {
        public Product Product { get; set; }

        public string Location { get; set; }

        public decimal Quantity { get; set; }
        
        public override string ToString()
        {
            return $"ProductCode: {Product.Code} | Location: {Location} | Quantity: {Quantity}";
        }
        
        public override bool Equals(object obj)
        {
            if(obj.GetType() != typeof(Stock))
                return false;
                
            var stock = (Stock)obj;
            return stock.Product.Code.Equals(this.Product.Code)
                && stock.Location.Equals(this.Location)
                && stock.Quantity.Equals(this.Quantity);
        }
        
        public override int GetHashCode()
        {
            //http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
            unchecked
            {
                int hash = (int) 2166136261;
                hash = (hash * 16777619) ^ Product.Code.GetHashCode();
                hash = (hash * 16777619) ^ Location.GetHashCode();
                hash = (hash * 16777619) ^ Quantity.GetHashCode();
                return hash;
            }
        }
    }
}