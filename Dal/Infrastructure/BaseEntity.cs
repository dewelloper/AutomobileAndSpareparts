using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Infrastructure
{
    public interface IUniqueable
    {
        int Id { get; set; }
    }

    public abstract class Entity : IUniqueable
    {
        protected Entity()
        { }

        [Key]
        public virtual int Id { get; set; }
    }

    public interface IUUniqueable
    {
        int UserId { get; set; }
    }

    public abstract class Entity2 : IUUniqueable
    {
        protected Entity2()
        { }

        [Key]
        public virtual int UserId { get; set; }
    }

}
