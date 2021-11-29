using Dal;
using System.Collections.Generic;

namespace BusinessObjects
{
    public class OrderViewModel
    {
        public List<Products> listProduct { get; set; }
        public List<UserAdresses> listUserAddress { get; set; }
        public List<UserProfile> listUserProfile { get; set; }
        public List<Currencies> currencies { get; set; }
        public List<Cities> listCities { get; set; }
    }
}
