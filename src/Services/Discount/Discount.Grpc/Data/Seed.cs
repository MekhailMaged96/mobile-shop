using Discount.Grpc.Entities;
using Discount.Grpc.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Data
{
    public class Seed
    {
        public static async Task SeedCoupon(IUnitOfWork unitOfWork)
        {

            var coupons = new List<Coupon>
            {
                new Coupon { ProductName = "IPhone X", Description = "IPhone Discount", Amount = 150 },
                new Coupon { ProductName = "Samsung 10", Description = "Samsung Discount", Amount = 100 }
            };

            foreach (var item in coupons)
            {
                await unitOfWork.DiscountRepository.CreateDiscount(item);
            }

            await unitOfWork.Save();
        }
    }
}
