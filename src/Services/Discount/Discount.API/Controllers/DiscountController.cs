using Discount.API.Entities;
using Discount.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountController(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }


        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            var coupon = await _unitOfWork.DiscountRepository.GetDiscount(productName);
            if (coupon == null)
            {
                return NotFound();
            }
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            await _unitOfWork.DiscountRepository.CreateDiscount(coupon);
            await _unitOfWork.Save();
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
        {
            var coupondb = await _unitOfWork.DiscountRepository.GetDiscount(coupon.ProductName);
            if (coupondb == null)
            {
                return NotFound();
            }
            coupondb.Amount = coupon.Amount;
            coupondb.Description = coupon.Description;
          
            _unitOfWork.DiscountRepository.UpdateDiscount(coupondb);
             await _unitOfWork.Save();

            return Ok(coupon);
        }
      
        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {

            var coupondb = await _unitOfWork.DiscountRepository.GetDiscount(productName);
            if (coupondb == null)
            {
                return NotFound();
            }
             _unitOfWork.DiscountRepository.DeleteDiscount(coupondb);
            var result = await _unitOfWork.Save();

            return Ok(result);
        }










    }
}
