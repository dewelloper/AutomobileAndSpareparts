using Dal;
using Otomotivist.Domain.Repository;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Service.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otomotivist.Service.services
{
    public class OrderService : IOrderService
    {

        private readonly IUnitOfWork _uow;
        private readonly IGRepository<Orders> _orders;
        private readonly IGRepository<OrderDetails> _orderDetails;
        private readonly IGRepository<Bills> _bills;
        private readonly IGRepository<Products> _products;

        public OrderService(IUnitOfWork uow)
        {
            _uow = uow;
            _orders = _uow.GetRepository<Orders>();
            _orderDetails = _uow.GetRepository<OrderDetails>();
            _bills = _uow.GetRepository<Bills>();
            _products = _uow.GetRepository<Products>();
        }

        public IQueryable<Orders> GetOrders(int RequesterUserId)
        {
            return _orders.Where(k => k.RequesterUserId == RequesterUserId).AsQueryable();
        }

        public IQueryable<Products> GetContainerProducts(List<Int64> containerIds)
        {
            return _products.Where(k => containerIds.Contains(k.Id)).AsQueryable();
        }

        public bool InsertOrder(Orders ord)
        {
            _orders.Insert(ord);
            _uow.SavaChange();
            return true;
        }

        public bool InsertOrderDetail(OrderDetails orderdetail)
        {
            _orderDetails.Insert(orderdetail);
            _uow.SavaChange();
            return true;
        }

        public bool InsertBills(Bills bills)
        {
            _bills.Insert(bills);
            _uow.SavaChange();
            return true;
        }

    }
}
