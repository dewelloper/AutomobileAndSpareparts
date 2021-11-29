using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Otomotivist.Domain.Repository
{
    public interface IGRepository<TEntity> where TEntity : class
    {
        TEntity Single(int? id);
        TEntity Single(long? id);
        TEntity Single(string where = null, params object[] parms);
        IEnumerable<TEntity> All();
        IEnumerable<TEntity> All(string ids);
        IEnumerable<TEntity> All(string where = null, string orderBy = null, int top = 0, params object[] parms);
        IEnumerable<TEntity> Paged(out int totalRows, string where = null, string orderBy = null, int page = 0, int pageSize = 20, params object[] parms);
        void Insert(TEntity t);
        void Update(TEntity t);
        void Delete(TEntity t);
        object Scalar(string operation, string column, string where = null, params object[] parms);
        int Count(string where = null, params object[] parms);
        object Max(string column = null, string where = null, params object[] parms);
        object Min(string column = null, string where = null, params object[] parms);
        object Sum(string column, string where = null, params object[] parms);
        IEnumerable<TEntity> Query(string sql, params object[] parms);
        int Execute(string sql, params object[] parms);

        DbSet<TEntity> GetContext();
        string GetConnectionString();
        int GetCountByAdoNet(string query);

        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> Filter = null);

    }
}
