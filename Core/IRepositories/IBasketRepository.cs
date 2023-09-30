using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.IRepositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetCustomerBasketAsync(string basketId);

        Task<CustomerBasket> UpdateCustomerBasketAsync(CustomerBasket customerBasket);

        Task<bool> DeleteCustomerBasketAsync(string basketId);
    }
}