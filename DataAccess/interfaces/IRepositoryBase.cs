using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();

        IQueryable<T> FinByCondition(Expression<Func<T, bool>> expression);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);


    }
}
