using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SqlClient;
using Dal;
using System.Linq.Expressions;

namespace Otomotivist.Domain.Repository
{
    public class GRepository<TEntity> : IGRepository<TEntity> where TEntity : class
    {
        private readonly Entities _context;
        private readonly DbSet<TEntity> _dbSet;

        public GRepository(Entities context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public TEntity Single(int? id)
        {
            return _dbSet.Find(id);
        }

        public TEntity Single(long? id)
        {
            return _dbSet.Find(id);
        }

        public TEntity Single(string where = null, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> All()
        {
            return _dbSet;
        }

        public IEnumerable<TEntity> All(string ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> All(string where = null, string orderBy = null, int top = 0, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Paged(out int totalRows, string where = null, string orderBy = null, int page = 0, int pageSize = 20, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity t)
        {
            _dbSet.Add(t);
            _context.SaveChanges();
        }

        public void Update(TEntity t)
        {
            _dbSet.Attach(t);
            _context.Entry<TEntity>(t).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(TEntity t)
        {
            if (_context.Entry<TEntity>(t).State == EntityState.Detached)
                _dbSet.Attach(t);
            _dbSet.Remove(t);
            _context.SaveChanges();
        }

        public object Scalar(string operation, string column, string where = null, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public int Count(string where = null, params object[] parms)
        {
            return _dbSet.Count();
        }

        public object Max(string column = null, string where = null, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public object Min(string column = null, string where = null, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public object Sum(string column, string where = null, params object[] parms)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Query(string sql, params object[] parms)
        {
            //return _context.Database.SqlQuery<TEntity>(sql, parms);
            return _dbSet.SqlQuery(sql, parms);
        }

        public int Execute(string sql, params object[] parms)
        {
            return _context.Database.ExecuteSqlCommand(sql, parms);
        }

        public DbSet<TEntity> GetContext()
        {
            return _dbSet;
        }

        public string GetConnectionString()
        {
            return _context.Database.Connection.ConnectionString;
        }

        public int GetCountByAdoNet(string query)
        {
            using (SqlConnection connection = new SqlConnection(_context.Database.Connection.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query.Replace("*", " COUNT(*) "), connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return Convert.ToInt32(reader[0]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {

                }
            }
            return 0;
        }

        public virtual IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> Filter = null)
        {
            if (Filter != null)
            {
                return _dbSet.Where(Filter);
            }
            return _dbSet;
        }

    }
}
