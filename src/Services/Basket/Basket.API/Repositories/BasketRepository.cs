using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketRepository(IDistributedCache distributedCache, DiscountGrpcService discountGrpcService )
        {
            _redisCache = distributedCache;
            _discountGrpcService = discountGrpcService;
        }


        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            decimal totalPrice=0;
            // Call Grpc Service To Get Discount 
            foreach(var item in basket.Items)
            {
                var discount = await _discountGrpcService.GetDiscount(item.ProductName);

                if(discount != null)
                {
                    item.Price -= discount.Amount;
                }

                totalPrice += item.Price * item.Quantity;
            }

            basket.TotalPrice = totalPrice;

            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

      

       
    }
}
