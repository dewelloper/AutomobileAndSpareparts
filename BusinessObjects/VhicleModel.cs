using Dal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BusinessObjects
{
    public class VhicleModel
    {
        public List<Products> products { get; set; }
        public List<Categories> listCatG { get; set; }
        public List<Currencies> currencies { get; set; }
        public List<Cities> cities { get; set; }
        public List<ProductGroups> listpG { get; set; }
        public List<Marks> listMarks { get; set; }
    }
}