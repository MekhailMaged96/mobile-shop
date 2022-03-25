using Discount.API.Data;
using Discount.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly DataContext _dataContext;

        public DiscountRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task CreateDiscount(Coupon coupon)
        {
             await _dataContext.Coupons.AddAsync(coupon);
            
        }
        public async Task<Coupon> GetDiscount(string productName)
        {
            return await _dataContext.Coupons.FirstOrDefaultAsync(e => e.ProductName.ToLower() == productName.ToLower());
        }

        public void UpdateDiscount(Coupon coupon)
        {
            _dataContext.Entry(coupon).State = EntityState.Modified;
        }

        public void DeleteDiscount(Coupon coupon)
        {
            _dataContext.Coupons.Remove(coupon);

        }
    }
}
