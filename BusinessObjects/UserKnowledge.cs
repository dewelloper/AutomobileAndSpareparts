using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessObjects
{
    public class UserMKnowledge
    {

        int _id = 0;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        string _eMail = "";

        public string EMail
        {
            get { return _eMail; }
            set { _eMail = value; }
        }

        bool _isConfirmed = false;

        public bool IsConfirmed
        {
            get { return _isConfirmed; }
            set { _isConfirmed = value; }
        }

        string _userName = "";

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

    }
}