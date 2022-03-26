using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService: DiscountProtoService.DiscountProtoServiceBase
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public DiscountService(IUnitOfWork unitOfWork,ILogger<DiscountService> logger,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _unitOfWork.DiscountRepository.GetDiscount(request.ProductName);
         
            _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);


            await _unitOfWork.DiscountRepository.CreateDiscount(coupon);
            await _unitOfWork.Save();

            _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;

        }


        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var mappedcoupon = _mapper.Map<Coupon>(request.Coupon);

            var coupondb = await _unitOfWork.DiscountRepository.GetDiscount(request.Coupon.ProductName);

            if (coupondb == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.Coupon.ProductName} is not found."));
            }

            coupondb.Description = mappedcoupon.Description;
            coupondb.Amount = mappedcoupon.Amount;

            _unitOfWork.DiscountRepository.UpdateDiscount(coupondb);
            await _unitOfWork.Save();

            _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupondb.ProductName);

            var couponModel = _mapper.Map<CouponModel>(coupondb);

            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupondb = await _unitOfWork.DiscountRepository.GetDiscount(request.ProductName);

            if (coupondb == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }


            _unitOfWork.DiscountRepository.DeleteDiscount(coupondb);

            var result = await _unitOfWork.Save();

            var deleteDiscountResponse = new DeleteDiscountResponse() {

                Success = result
            };

            return deleteDiscountResponse;

        }
    }
}
