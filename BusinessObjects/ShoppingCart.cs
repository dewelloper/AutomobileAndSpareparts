using System.Collections.Generic;
using Dal;

namespace BusinessObjects
{
    public class ShoppingCart
    {
        private List<OrderDetails> _items = new List<OrderDetails>();
        public List<OrderDetails> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }

        private Orders _order = new Orders();
        public Orders Order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
            }
        }

        private decimal _discount = 0;
        public decimal Discount
        {
            get
            {
                return _discount;
            }
            set
            {
                _discount = value;
            }
        }
    }
}
