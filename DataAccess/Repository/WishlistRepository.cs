﻿using Domain.interfaces.Repository;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class WishlistRepository : RepositoryBase<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(FlowersStoreContext repositoriContext)
: base(repositoriContext)
        {

        }
    }
}
