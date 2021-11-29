using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otomotivist.Service.interfaces
{
    public interface IOrderService
    {
        IQueryable<Orders> GetOrders(int RequesterUserId);
        IQueryable<Products> GetContainerProducts(List<Int64> containerIds);
        bool InsertOrder(Orders ord);
        bool InsertOrderDetail(OrderDetails orderdetail);
        bool InsertBills(Bills bills);
    }
}
