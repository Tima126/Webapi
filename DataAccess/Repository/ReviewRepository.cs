using DataAccess.interfaces;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ReviewRepository:RepositoryBase<Review>,IReviewRepository
    {
        public ReviewRepository(FlowersStoreContext repositoriContext)
        : base(repositoriContext)
        {

        }
    }
}
