using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository basketRepository,IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }


        [HttpGet("{userName}",Name ="GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart shoppingCart)
        {
            var basket = await _basketRepository.UpdateBasket(shoppingCart);

            return Ok(basket);
        }


        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);

            return Ok();
        }


        [HttpPost]
        [Route("Checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ShoppingCart>> Checkout(BasketCheckout basketCheckout)
        {

            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);

            if(basket == null)
            {
                return BadRequest();
            }

            var basketEvent = _mapper.Map<BasketCheckoutEvent>(basketCheckout);

            basketEvent.TotalPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(basketEvent);

            await _basketRepository.DeleteBasket(basket.UserName);

            return Accepted();
        }
    }
}
