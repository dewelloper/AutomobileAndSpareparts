using Dal;
using Otomotivist.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otomotivist.Domain.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Entities _context;
        private bool disposed = false;

        public UnitOfWork(Entities context)
        {
            Database.SetInitializer<Entities>(null);
            if (context == null)
                throw new ArgumentException("context is null");
            _context = context;
        }

        public void BeginTransaction()
        {
            _context.Database.Connection.BeginTransaction();
        }

        public void Commit()
        {
            Commit();
        }

        public void Rollback()
        {
            Rollback();
        }

        public int SavaChange()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        public IGRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new GRepository<TEntity>(_context);
        }

    }
}
