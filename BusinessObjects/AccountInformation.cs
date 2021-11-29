using System;
using System.Collections.Generic;
using Dal;

namespace BusinessObjects
{
    public class AccountInformation
    {
        private int _userId = 0;
        public int UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        private string _userName = string.Empty;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        private int _userTypeId = 0;
        public int UserTypeId
        {
            get
            {
                return _userTypeId;
            }
            set
            {
                _userTypeId = value;
            }
        }

        private int _userMembershipId = 0;
        public int UserMembershipId
        {
            get
            {
                return _userMembershipId;
            }
            set
            {
                _userMembershipId = value;
            }
        }

        private string _userProvider = string.Empty;
        public string UserProvider
        {
            get
            {
                return _userProvider;
            }
            set
            {
                _userProvider = value;
            }
        }

        private List<int> _userRoleIds = new List<int>();
        public List<int> UserRoleIds
        {
            get
            {
                return _userRoleIds;
            }
            set
            {
                _userRoleIds = value;
            }
        }

        private List<int> _userInRolesIds = new List<int>();
        public List<int> UserInRolesIds
        {
            get
            {
                return _userInRolesIds;
            }
            set
            {
                _userInRolesIds = value;
            }
        }

        private List<Orders> _userOrders = new List<Orders>();
        public List<Orders> UserOrders
        {
            get
            {
                return _userOrders;
            }
            set
            {
                _userOrders = value;
            }
        }

        private List<Products> _userProducts = new List<Products>();
        public List<Products> UserProducts
        {
            get
            {
                return _userProducts;
            }
            set
            {
                _userProducts = value;
            }
        }

        private List<Galleries> _userGalleries = new List<Galleries>();
        public List<Galleries> UserGalleries
        {
            get
            {
                return _userGalleries;
            }
            set
            {
                _userGalleries = value;
            }
        }

        private List<AutoServices> _userAutoServices = new List<AutoServices>();
        public List<AutoServices> UserAutoServices
        {
            get
            {
                return _userAutoServices;
            }
            set
            {
                _userAutoServices = value;
            }
        }

        private List<UserAdresses> _adress = new List<UserAdresses>();
        public List<UserAdresses> Adress
        {
            get
            {
                return _adress;
            }
            set
            {
                _adress = value;
            }
        }

        private UserAdresses _selectedAdress = null;
        public UserAdresses SelectedAdress
        {
            get
            {
                return _selectedAdress;
            }
            set
            {
                _selectedAdress = value;
            }
        }

        private string _userProfilePhoto = string.Empty;
        public string UserProfilePhoto
        {
            get
            {
                return _userProfilePhoto;
            }
            set
            {
                _userProfilePhoto = value;
            }
        }

        private string _userRealName = string.Empty;
        public string UserRealName
        {
            get
            {
                return _userRealName;
            }
            set
            {
                _userRealName = value;
            }
        }

        private string _userSurname = string.Empty;
        public string UserSurname
        {
            get
            {
                return _userSurname;
            }
            set
            {
                _userSurname = value;
            }
        }

        private Nullable<bool> _userGender = new bool();
        public Nullable<bool> UserGender
        {
            get
            {
                return _userGender;
            }
            set
            {
                _userGender = value;
            }
        }

        private string _userEducationLevel = string.Empty;
        public string UserEducationLevel
        {
            get
            {
                return _userEducationLevel;
            }
            set
            {
                _userEducationLevel = value;
            }
        }

        private string _userJob = string.Empty;
        public string UserJob
        {
            get
            {
                return _userJob;
            }
            set
            {
                _userJob = value;
            }
        }

        private List<UserGender> _userGenderList = new List<UserGender>();
        public List<UserGender> UserGenderList
        {
            get
            {
                return _userGenderList;
            }
            set
            {
                _userGenderList = value;
            }
        }

        private List<UserEducationLevel> _userEducationLevelList = new List<UserEducationLevel>();
        public List<UserEducationLevel> UserEducationLevelList
        {
            get
            {
                return _userEducationLevelList;
            }
            set
            {
                _userEducationLevelList = value;
            }
        }

        private List<UserJob> _userJobList = new List<UserJob>();
        public List<UserJob> UserJobList
        {
            get
            {
                return _userJobList;
            }
            set
            {
                _userJobList = value;
            }
        }

        private List<ProductGroups> _prdcGrpList = new List<ProductGroups>();
        public List<ProductGroups> PrdcGrpList
        {
            get
            {
                return _prdcGrpList;
            }
            set
            {
                _prdcGrpList = value;
            }
        }

        private List<Categories> _prdcCatList = new List<Categories>();
        public List<Categories> PrdcCatList
        {
            get
            {
                return _prdcCatList;
            }
            set
            {
                _prdcCatList = value;
            }
        }

        private List<Marks> _prdcMarkList = new List<Marks>();
        public List<Marks> PrdcMarkList
        {
            get
            {
                return _prdcMarkList;
            }
            set
            {
                _prdcMarkList = value;
            }
        }

        private List<Currencies> _prdcCurrList = new List<Currencies>();
        public List<Currencies> PrdcCurrList
        {
            get
            {
                return _prdcCurrList;
            }
            set
            {
                _prdcCurrList = value;
            }
        }

        private string _userCellPhone = string.Empty;
        public string UserCellPhone
        {
            get
            {
                return _userCellPhone;
            }
            set
            {
                _userCellPhone = value;
            }
        }

        private string _userHomePhone = string.Empty;
        public string UserHomePhone
        {
            get
            {
                return _userHomePhone;
            }
            set
            {
                _userHomePhone = value;
            }
        }

        private string _userWorkPhone = string.Empty;
        public string UserWorkPhone
        {
            get
            {
                return _userWorkPhone;
            }
            set
            {
                _userWorkPhone = value;
            }
        }

        private string _userFaxNumber = string.Empty;
        public string UserFaxNumber
        {
            get
            {
                return _userFaxNumber;
            }
            set
            {
                _userFaxNumber = value;
            }
        }

        private string _userTcId = string.Empty;
        public string UserTcId
        {
            get
            {
                return _userTcId;
            }
            set
            {
                _userTcId = value;
            }
        }

        private DateTime _userBirthDate = DateTime.Now;
        public DateTime UserBirthDate
        {
            get
            {
                return _userBirthDate;
            }
            set
            {
                _userBirthDate = value;
            }
        }

        private string _userEmail = string.Empty;
        public string UserEmail
        {
            get
            {
                return _userEmail;
            }
            set
            {
                _userEmail = value;
            }
        }

        private List<UserMessages> _userMessages = new List<UserMessages>();

        public List<UserMessages> UserMessages
        {
            get { return _userMessages; }
            set { _userMessages = value; }
        }

        private List<UserFeedbacks> _userFeedBacks = new List<UserFeedbacks>();

        public List<UserFeedbacks> UserFeedBacks
        {
            get { return _userFeedBacks; }
            set { _userFeedBacks = value; }
        }
    }
}
