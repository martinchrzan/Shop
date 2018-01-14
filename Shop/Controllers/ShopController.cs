using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopModels;
using Shop.Data;
using Microsoft.EntityFrameworkCore;
using Shop.Payment;

namespace Shop.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IPaymentProvider _paymentProvider;
        private readonly UsersController _userController;

        public ShopController(DataContext dataContext, IPaymentProvider paymentProvider)
        {
            _dataContext = dataContext;
            _paymentProvider = paymentProvider;
            _userController = new UsersController(_dataContext);
        }

        [HttpGet("ListProducts")]
        public List<Product> ListProducts()
        {
            return _dataContext.Products.ToList();
        }

        [HttpGet("GetCustomerProducts/{userToken}")]
        public Basket GetCustomerProducts(string userToken)
        {
            if(string.IsNullOrEmpty(userToken))
            {
                return new Basket();
            }

            return _dataContext.Baskets.Include(it=> it.Products).FirstOrDefault(it => it.UserToken == userToken) ?? new Basket();
        }

        [HttpPost("AddCustomerProduct")]
        public bool AddCustomerProduct([FromBody]BasketItem basketItem)
        {
            if(ModelState.IsValid)
            {
                return ChangeCountOfProduct(basketItem, true);
            }
            return false;
        }

        [HttpPost("RemoveCustomerProduct")]
        public bool RemoveCustomerProduct([FromBody]BasketItem basketItem)
        {
            if (ModelState.IsValid)
            {
                return ChangeCountOfProduct(basketItem, false);
            }
            return false;
        }

        [HttpPost("ClearProducts/{userToken}")]
        public bool ClearProducts(string userToken)
        {
            if(string.IsNullOrEmpty(userToken) || !_dataContext.Baskets.Any(it=> it.UserToken == userToken))
            {
                return false;
            }

            _dataContext.Baskets.Include(it=> it.Products).First(it => it.UserToken == userToken).Products.Clear();
            _dataContext.SaveChanges();

            return true;
        }

        [HttpPost("PurchaseProducts/{userToken}")]
        public bool PurchaseProducts(string userToken)
        {
            var result = _paymentProvider.ProcessPayment(userToken);
            if(result)
            {
                ClearProducts(userToken);
            }
            return result;
        }
        
        private bool ChangeCountOfProduct(BasketItem basketItem, bool addition)
        {
            var basket = _dataContext.Baskets.Include(it => it.Products).FirstOrDefault(it => it.UserToken == basketItem.UserToken);
            if (basket == null || !_dataContext.Products.Any(it => it.Id == basketItem.ProductCount.ProductId))
            {
                // no basket setup for given customer -> customer does not exist
                // no such product exists
                return false;
            }

            var productCount = basket.Products.FirstOrDefault(it => it.ProductId == basketItem.ProductCount.ProductId);

            if (productCount != null)
            {
                if(addition)
                {
                    productCount.Count += basketItem.ProductCount.Count;
                }
                else
                {
                    productCount.Count -= basketItem.ProductCount.Count;
                    if(productCount.Count < 0)
                    {
                        productCount.Count = 0;
                    }
                }
            }
            else if(addition)
            {
                basket.Products.Add(basketItem.ProductCount);
            }
            else if (!addition)
            {
                return false;
            }

            _dataContext.SaveChanges();
            return true;
        }
    }
}