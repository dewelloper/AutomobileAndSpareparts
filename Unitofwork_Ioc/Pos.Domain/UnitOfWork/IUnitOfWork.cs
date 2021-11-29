using Otomotivist.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Otomotivist.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        int SavaChange();
        void BeginTransaction();
        void Commit();
        void Rollback();
        new void Dispose();
    }
}
