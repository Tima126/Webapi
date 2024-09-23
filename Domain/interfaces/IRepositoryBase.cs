using System;
using Domain.interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.interfaces
{
    public interface IRepositoryBase<T>
    {
        Task<List<T>> FindAll();
        Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }
}
