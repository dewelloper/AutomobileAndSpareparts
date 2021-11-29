using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessObjects
{
    public class EditablePropertyOfProduct
    {
        Int64 _id =0;

        public Int64 id
        {
            get { return _id; }
            set { _id = value; }
        }

        string _name = "";

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        bool? _isActive;

        public bool? IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        int? _categoryId = 0;

        public int? CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        //string _editOrDelete = "";

        //public string EditOrDelete
        //{
        //    get { return _editOrDelete; }
        //    set { _editOrDelete = value; }
        //}
    }
}