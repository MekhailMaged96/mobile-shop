using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IDiscountRepository DiscountRepository { get; }
        Task<bool> Save();
    }
}
