using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc4WebRole.Areas.Admin.Models
{
    public class AreaotomobilvasitaViewModel
    {
        //public AreaotomobilvasitaViewModel()
        //{
        //    _product.Add(new Product() { Id = 1, Name = "test", IsActive = true });
        //}


        List<EditablePropertyOfProduct> _product = new List<EditablePropertyOfProduct>();

        public List<EditablePropertyOfProduct> Product
        {
            get { return _product; }
            set { _product = value; }
        }

        public bool IsEmpty()
        {
            return _product.Count > 0;
        }

    }
}