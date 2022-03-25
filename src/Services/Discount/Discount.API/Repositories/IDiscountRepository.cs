using Discount.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task CreateDiscount(Coupon coupon);
        void UpdateDiscount(Coupon coupon);
        void DeleteDiscount(Coupon coupon);
    }
}
