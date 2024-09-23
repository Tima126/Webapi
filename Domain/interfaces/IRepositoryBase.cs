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

        Task<List<T>> FinByCondition(Expression<Func<T, bool>> expression);

        Task<List<T>> Create(T entity);
        Task<List<T>> Update(T entity);
        Task<List<T>> Delete(T entity);


    }
}
