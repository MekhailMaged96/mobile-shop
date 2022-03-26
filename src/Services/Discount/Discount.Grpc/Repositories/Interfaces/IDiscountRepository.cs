using Discount.Grpc.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task CreateDiscount(Coupon coupon);
        void UpdateDiscount(Coupon coupon);
        void DeleteDiscount(Coupon coupon);
    }
}
