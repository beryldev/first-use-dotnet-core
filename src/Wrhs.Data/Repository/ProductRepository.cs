using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Data.Repository
{
    public class ProductRepository : IRepository<Product>, IDisposable
    {
        WrhsContext context;

        public ProductRepository(WrhsContext context)
        {
            this.context = context;
        }

        public IEnumerable<Product> Get()
        {
            return context.Products;
        }

        public Product GetById(int id)
        {
            return context.Products
                .Where(prod=>prod.Id == id)
                .FirstOrDefault();
        }

        public Product Save(Product item)
        {
            context.Products.Add(item);
            context.SaveChanges();

            return item;
        }

        public void Delete(Product item)
        {
            context.Products.Remove(item);
            context.SaveChanges();
        }

        
        public void Update(Product item)
        {
            context.Update(item);
            context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ProductRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}