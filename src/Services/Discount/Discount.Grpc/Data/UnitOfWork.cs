using Discount.Grpc.Repositories;
using Discount.Grpc.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IDiscountRepository DiscountRepository => new DiscountRepository(_dataContext);

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }


    }
}
