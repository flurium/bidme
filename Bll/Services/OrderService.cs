﻿using Dal.UnitOfWork;
using Domain.Models;
using System.Linq.Expressions;

namespace Bll.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(Order order)
        {
            if (order != null)
            {
                await _unitOfWork.OrderRepository.Create(order);
            }
        }

        public async Task DeleteAsync(string uId, int pId)
        {
            await _unitOfWork.OrderRepository.DeleteAsync(uId, pId);
        }

        public async Task<IReadOnlyCollection<Order>> FindIncludeProductsAsync(Expression<Func<Order, bool>> conditon)
        {
            return await _unitOfWork.OrderRepository.FindIncludeProductsAsync(conditon);
        }

        public async Task<Order?> Get(string uId, int pId)
        {
            return await _unitOfWork.OrderRepository.GetById(uId, pId);
        }

        public async Task<IReadOnlyCollection<Order>> FindByConditionAsync(Expression<Func<Order, bool>> conditon)
        {
            return await _unitOfWork.OrderRepository.FindByConditionAsync(conditon);
        }
    }
}