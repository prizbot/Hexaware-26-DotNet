using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using orders_ecommerce_demo.Model;
using orders_ecommerce_demo.Service;

namespace orders_ecommerce_demo.Tests
{
    [TestFixture]
    public class OrderBillingServiceTest
    {
        private OrderBillingService _orderBillingService;
        private Order _order;
        [SetUp]
        public void Setup()
        {
            _orderBillingService = new OrderBillingService();
            _order = new Order
            {
                Price = 250,
                Quantity = 6
            };
        }
        [Test]
        public void CalculateSubTotal_ValidInput_ReturnsSubTotal()
        {
            decimal result=_orderBillingService.CalculateSubTotal(2500,2);
            Assert.That(result, Is.EqualTo(5000));
        }
        [Test]
        public void CalcluateSubTotal_PriceLessThanOrEqualToZero_ThrowsException()
        {
            Assert.Throws
        }
    }
}
