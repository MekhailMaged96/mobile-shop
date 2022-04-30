using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.BasketService;
using Shopping.Aggregator.Services.CatalogService;
using Shopping.Aggregator.Services.OrderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _orderService = orderService;
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetShopping(string userName)
        {
            var basket = await _basketService.GetBasket(userName);
            var shoppingModel = new ShoppingModel();

            shoppingModel.UserName = userName;
            shoppingModel.BasketWithProducts = basket;

            if (basket.Items != null)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _catalogService.GetCatalog(item.ProductId);

                    // set additional product fields onto basket item
                    item.ProductName = product.Name;
                    item.Category = product.Category;
                    item.Summary = product.Summary;
                    item.Description = product.Description;
                    item.ImageFile = product.ImageFile;
                }
                var orders = await _orderService.GetOrdersByUserName(userName);
                shoppingModel.Orders = orders;

            }

            return Ok(shoppingModel);
        }

    }
}
