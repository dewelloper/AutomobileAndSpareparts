using Dal;
using Otomotivist.Domain.Repository;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Service.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Otomotivist.Service.services
{

    public class OtomotivistCoreService : IOtomotivistCoreService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGRepository<Products> _products;
        private readonly IGRepository<UserMessages> _userMessages;
        private readonly IGRepository<Categories> _categories;
        private readonly IGRepository<Cities> _cities;
        private readonly IGRepository<FuelTypes> _fuelTypes;
        private readonly IGRepository<CaseTypes> _caseTypes;
        private readonly IGRepository<GearTypes> _gearTypes;
        private readonly IGRepository<ProductGroups> _productGroups;
        private readonly IGRepository<Towns> _towns;
        private readonly IGRepository<SubDistrict> _subDistrict;
        private readonly IGRepository<Marks> _marks;
        private readonly IGRepository<Denominations> _denominations;
        private readonly IGRepository<ProductStates> _productStates;
        private readonly IGRepository<Currencies> _currencies;
        private readonly IGRepository<Colors> _colors;
        private readonly IGRepository<DamageStates> _damageStates;
        private readonly IGRepository<ModelYears> _modelYears;
        private readonly IGRepository<EnginePowers> _enginePowers;
        private readonly IGRepository<EngineVolumes> _engineVolumes;
        private readonly IGRepository<VehicleTypes> _vehicleTypes;
        private readonly IGRepository<PlateNationalities> _plateNationalities;
        private readonly IGRepository<TractionTypes> _tractionTypes;
        private readonly IGRepository<TransportTypes> _transportTypes;
        private readonly IGRepository<GuarantySituations> _guarantySituations;
        private readonly IGRepository<PublishDurations> _publishDurations;
        private readonly IGRepository<ProductSeller> _productSeller;
        private readonly IGRepository<UserProfile> _userProfile;
        private readonly IGRepository<UserAdresses> _userAdresses;
        private readonly IGRepository<UserFeedbacks> _userFeedbacks;
        private readonly IGRepository<UserEducationLevel> _userEducationLevel;
        private readonly IGRepository<UserJob> _userJob;
        private readonly IGRepository<UserGender> _userGender;
        private readonly IGRepository<AutoServices> _autoservices;
        private readonly IGRepository<Galleries> _galleries;
        private readonly IGRepository<Orders> _orders; 
        

        public OtomotivistCoreService(UnitOfWork uow)
        {
            _uow = uow;
            _products = _uow.GetRepository<Products>();
            _userMessages = _uow.GetRepository<UserMessages>();
            _categories = _uow.GetRepository<Categories>();
            _cities = _uow.GetRepository<Cities>();
            _fuelTypes = _uow.GetRepository<FuelTypes>();
            _caseTypes = _uow.GetRepository<CaseTypes>();
            _gearTypes = _uow.GetRepository<GearTypes>();
            _productGroups = _uow.GetRepository<ProductGroups>();
            _towns = _uow.GetRepository<Towns>();
            _subDistrict = _uow.GetRepository<SubDistrict>();
            _marks = _uow.GetRepository<Marks>();
            _denominations = _uow.GetRepository<Denominations>();
            _productStates = _uow.GetRepository<ProductStates>();
            _currencies = _uow.GetRepository<Currencies>();
            _colors = _uow.GetRepository<Colors>();
            _damageStates = _uow.GetRepository<DamageStates>();
            _modelYears = _uow.GetRepository<ModelYears>();
            _enginePowers = _uow.GetRepository<EnginePowers>();
            _engineVolumes = _uow.GetRepository<EngineVolumes>();
            _vehicleTypes = _uow.GetRepository<VehicleTypes>();
            _plateNationalities = _uow.GetRepository<PlateNationalities>();
            _tractionTypes = _uow.GetRepository<TractionTypes>();
            _transportTypes = _uow.GetRepository<TransportTypes>();
            _guarantySituations = _uow.GetRepository<GuarantySituations>();
            _publishDurations = _uow.GetRepository<PublishDurations>();
            _productSeller = _uow.GetRepository<ProductSeller>();
            _userProfile = _uow.GetRepository<UserProfile>();
            _userAdresses = _uow.GetRepository<UserAdresses>();
            _userFeedbacks = _uow.GetRepository<UserFeedbacks>();
            _userEducationLevel = _uow.GetRepository<UserEducationLevel>();
            _userJob = _uow.GetRepository<UserJob>();
            _userGender = _uow.GetRepository<UserGender>();
            _autoservices = _uow.GetRepository<AutoServices>();
            _galleries = _uow.GetRepository<Galleries>();
            _orders = _uow.GetRepository<Orders>();
        }

        public IQueryable<Products> NewAutomobilesTop30()
        {
            var prdcs = _products.Where(k => k.CurrentPrice > 0 && k.ProductType == 1 && k.IsActive == true && k.ImagePath0 != null && k.ImagePath0 != "")
                        .OrderByDescending(l => l.Id)
                        .Take(30)
                        .AsQueryable();
            return prdcs;
        }

        public IQueryable<Products> NewSparepartsTop30()
        {
            var sps = _products.Where(m => m.CurrentPrice > 0 & m.ProductType == 2 && m.IsActive == true && m.ImagePath0 != null && m.ImagePath0 != "")
                        .OrderByDescending(n => n.Id)
                        .Take(30)
                        .AsQueryable();
            return sps;
        }

        public IQueryable<Products> GetProductsByTypeAndContainerCategory(int productType, List<int?> parentTypeIds)
        {
            return _products.Where(K => K.ProductType == productType && 
                K.IsActive == true &&
                parentTypeIds.Contains(K.CategoryId)).Take(500).AsQueryable();
        }

        public IQueryable<Products> GetProductsByTypeAndCriteriaCode(int productType, string criteriaCode)
        {
            return _products.Where(K => K.ProductType == productType && (criteriaCode.Contains(K.Code) || criteriaCode.Contains(K.Explanation)) && K.IsActive == true).Take(500).AsQueryable();
        }

        public IQueryable<Products> GetProductsByTypeAndCriteriaCodeAndContainer(int productType, string criteriaCode, List<int?> containerIds)
        {
            return _products.Where(K => K.ProductType == productType && K.Code == criteriaCode && K.IsActive == true && containerIds.Contains(K.CategoryId) 
                || criteriaCode == string.Empty || criteriaCode == string.Empty).Take(500).AsQueryable();
        }

        public IQueryable<Products> GetProductsByType(int productType)
        {
            return _products.Where(K => K.ProductType == productType && K.IsActive == true).Take(500).AsQueryable();
        }

        public IQueryable<ProductGroups> GetProductroupById(int id)
        {
            if(id == 1)
                return _productGroups.All().AsQueryable();
            return _productGroups.Where(k => k.Id == id).AsQueryable();
        }

        public IQueryable<Categories> GetCategoriesById(int parentId)
        {
            return _categories.Where(k => k.ParentId == parentId).AsQueryable();
        }

        public IQueryable<Categories> GetCategoriesById(Int64 id)
        {
            return _categories.Where(k => k.Id == id).AsQueryable();
        }

        public IQueryable<Categories> GetCategoriesByIdIfProductExist(int id)
        {
            var cats = GetCategoriesById(id);
            var c2 = new List<Categories>();
            foreach (Categories c in cats)
            {
                var prd = _products.Where(k => k.CategoryId == c.Id).FirstOrDefault();
                if (prd != null)
                {
                    c2.Add(c);
                }
            }
            return c2.AsQueryable();
        }

        public IQueryable<Categories> GetCategoriesByContainer(List<int?> containerIds)
        {
            return _categories.Where(k => containerIds.Contains(k.ParentId)).AsQueryable();
        }       

        public IQueryable<Towns> GetTownsById(int id)
        {
            return _towns.Where(k => k.City_Id == id).AsQueryable();
        }

        public IQueryable<SubDistrict> GetSubdistrictsById(int id)
        {
            return  _subDistrict.Where(k => k.TownId == id).AsQueryable();
        }

        public UserProfile GetUserProfileById(int userid)
        {
            return _userProfile.Where(k => k.UserId == userid).FirstOrDefault();
        }

        public UserProfile GetUserProfileByName(string username)
        {
            UserProfile up = _userProfile.Where(k => k.UserName == username).FirstOrDefault();
            if(up == null)
                up = _userProfile.Where(k => k.eMail == username).FirstOrDefault();
            return up;
        }

        public IQueryable<UserAdresses> GetUserAddressesById(int addressId)
        {
            return _userAdresses.Where(k => k.Id == addressId).AsQueryable();
        }

        public IQueryable<UserAdresses> GetUserAddressesByUserId(int userid)
        {
            return _userAdresses.Where(k => k.UserId == userid).AsQueryable();
        }

        public IQueryable<UserFeedbacks> GetUserUserFeedbacksById(int userid)
        {
            return _userFeedbacks.Where(k => k.UserId == userid).AsQueryable();
        }

        public IQueryable<UserMessages> GetUserMessagesById(int userid)
        {
            return _userMessages.Where(k => k.ReceiverUserId == userid).AsQueryable();
        }

        public IQueryable<UserEducationLevel> GetUserEducationLevelsById(int? userid)
        {
            return _userEducationLevel.Where(k => k.Id == userid).AsQueryable();
        }

        public IQueryable<UserEducationLevel> GetUserEducationLevels()
        {
            return _userEducationLevel.All().AsQueryable();
        }

        public IQueryable<UserJob> GetUserJobsById(int? jid)
        {
            return _userJob.Where(_=> _.Id == jid).AsQueryable();
            //return _userJob.Where(k => k.id == jid).AsQueryable();
        }

        public IQueryable<UserGender> GetUserGenders()
        {
            return _userGender.All().AsQueryable();
        }

        public IQueryable<UserJob> GetUserJobs()
        {
            return _userJob.All().AsQueryable();
        }

        public IQueryable<AutoServices> Getautoservices(int registererId)
        {
            return _autoservices.Where(k => k.RegistererId == registererId).AsQueryable();
        }

        public IQueryable<Galleries> GetGalleries(int userId)
        {
            return _galleries.Where(k => k.Id == userId).AsQueryable();
        }

        public IQueryable<Orders> GetOrders(int RequesterUserId)
        {
            return _orders.Where(k => k.RequesterUserId == RequesterUserId).AsQueryable();
        }

        public IQueryable<Products> GetProducts(int RequesterUserId)
        {
            return _products.Where(k => k.RegistererId == RequesterUserId && k.IsActive != null && k.IsActive == true).OrderByDescending(m => m.Id).Take(40).AsQueryable();
        }

        public Products GetProductsById(int pid)
        {
            return _products.Where(k => k.Id == pid).FirstOrDefault();
        }

        public IQueryable<ProductGroups> GetProductGroups()
        {
            return _productGroups.All().AsQueryable();
        }

        public IQueryable<Categories> GetCategories()
        {
            return _categories.All().AsQueryable();
        }

        public IQueryable<Marks> GetMarks()
        {
            return _marks.All().AsQueryable();
        }

        public IQueryable<Currencies> GetCurrencies()
        {
            return _currencies.All().AsQueryable();
        }

        public IQueryable<ProductGroups> GetProductGroupsAll()
        {
            return _productGroups.All().AsQueryable();
        }

        public ProductGroups GetProductGroupsById(int productGroupId)
        {
            return _productGroups.Where(k=> k.Id == productGroupId).FirstOrDefault();
        }

        public IQueryable<Categories> GetCategoriesAll()
        {
            return _categories.All().AsQueryable();
        }

        public Categories GetCategoriesByCategoryId(int categoryId)
        {
            return _categories.Where(k => k.Id == categoryId).FirstOrDefault();
        }

        public IQueryable<Marks> GetMarksAll()
        {
            return _marks.All().AsQueryable();
        }

        public IQueryable<Denominations> GetDenominationsAll()
        {
            return _denominations.All().AsQueryable();
        }

        public IQueryable<ProductStates> GetProductStatesAll()
        {
            return _productStates.All().AsQueryable();
        }

        public IQueryable<Currencies> GetCurrenciesAll()
        {
            return _currencies.All().AsQueryable();
        }

        public Currencies GetCurrenciesById(int currId)
        {
            return _currencies.Where(k=> k.Id == currId).FirstOrDefault();
        }

        public IQueryable<Cities> GetCitiesAll()
        {
            return _cities.All().AsQueryable();
        }

        public Cities GetCitiesById(int id)
        {
            return _cities.Where(k=> k.Id == id).FirstOrDefault();
        }

        public IQueryable<CaseTypes> GetCaseTypesAll()
        {
            return _caseTypes.All().AsQueryable();
        }

        public CaseTypes GetCaseTypesById(int caseTypeId)
        {
            return _caseTypes.Where(k=> k.Id == caseTypeId).FirstOrDefault();
        }

        public IQueryable<Colors> GetColorsAll()
        {
            return _colors.All().AsQueryable();
        }

        public Colors GetColorsById(int colorId)
        {
            return _colors.Where(k => k.Id == colorId).FirstOrDefault();
        }

        public IQueryable<DamageStates> GetDamageStatesAll()
        {
            return _damageStates.All().AsQueryable();
        }

        public IQueryable<FuelTypes> GetFuelTypesAll()
        {
            return _fuelTypes.All().AsQueryable();
        }

        public FuelTypes GetFuelTypesById(int fuelTypeId)
        {
            return _fuelTypes.Where(k => k.Id == fuelTypeId).FirstOrDefault();
        }

        public IQueryable<ModelYears> GetModelYearsAll()
        {
            return _modelYears.All().AsQueryable();
        }

        public IQueryable<EnginePowers> GetEnginePowersAll()
        {
            return _enginePowers.All().AsQueryable();
        }

        public EnginePowers GetEnginePowersById(int enginePowerId)
        {
            return _enginePowers.Where(k => k.Id == enginePowerId).FirstOrDefault();
        }

        public IQueryable<EngineVolumes> GetEngineVolumesAll()
        {
            return _engineVolumes.All().AsQueryable();
        }

        public EngineVolumes GetEngineVolumesById(int engineVolumeId)
        {
            return _engineVolumes.Where(k => k.Id == engineVolumeId).FirstOrDefault();
        }

        public IQueryable<GearTypes> GetGearTypesAll()
        {
            return _gearTypes.All().AsQueryable();
        }

        public GearTypes GetGearTypesById(int gearTypeId)
        {
            return _gearTypes.Where(k=> k.Id == gearTypeId).FirstOrDefault();
        }

        public IQueryable<VehicleTypes> GetVehicleTypesAll()
        {
            return _vehicleTypes.All().AsQueryable();
        }

        public VehicleTypes GetVehicleTypesById(int vehicleTypeId)
        {
            return _vehicleTypes.Where(k => k.Id == vehicleTypeId).FirstOrDefault();
        }

        public IQueryable<PlateNationalities> GetPlateNationalitiesAll()
        {
            return _plateNationalities.All().AsQueryable();
        }

        public IQueryable<TractionTypes> GetTractionTypesAll()
        {
            return _tractionTypes.All().AsQueryable();
        }

        public IQueryable<TransportTypes> GetTransportTypesAll()
        {
            return _transportTypes.All().AsQueryable();
        }

        public IQueryable<GuarantySituations> GetGuarantySituationsAll()
        {
            return _guarantySituations.All().AsQueryable();
        }

        public IQueryable<PublishDurations> GetPublishDurationsAll()
        {
            return _publishDurations.All().AsQueryable();
        }

        public IQueryable<ProductSeller> GetProductSellersAll()
        {
            return _productSeller.All().AsQueryable();
        }

        public ProductSeller GetProductSellersById(int productSellerId)
        {
            return _productSeller.Where(k => k.Id == productSellerId).FirstOrDefault();
        }

        public IQueryable<Products> GetProductByCriteria(string criteria)
        {
            return _products.Where(k => k.Name.Contains(criteria) || k.Explanation.Contains(criteria) || k.Code.Contains(criteria) && k.IsActive == true).OrderByDescending(m => m.RecordDate).Take(500).AsQueryable();
        }

        public Cities GetCityById(int id)
        {
            return _cities.Where(k => k.Id == id).FirstOrDefault();
        }

        public string[] FindCategories(int? id)
        {
            var productCategory = _categories.Where(k => k.Id == id).FirstOrDefault();
            if (productCategory == null)
            {
                return null;
            }
            var rootLevel = productCategory.RootLevel;
            var productCategories = new string[rootLevel];
            var parentId = id;
            if (rootLevel == 1)
            {
                productCategories[0] = productCategory.Name;
            }
            else
            {
                for (var i = rootLevel; i > 0; i--)
                {
                    var prdcCat = _categories.Where(k => k.Id == parentId).FirstOrDefault();
                    productCategories[i - 1] = prdcCat.Name;
                    parentId = prdcCat.ParentId;
                }
            }
            return productCategories;
        }


        public bool InsertProduct(Products prd)
        {
            _products.Insert(prd);
            _uow.SavaChange();
            return true;
        }

        public bool InsertMessage(UserMessages msg)
        {
            _userMessages.Insert(msg);
            _uow.SavaChange();
            return true;
        }

        public bool InsertFeedBacks(UserFeedbacks ufb)
        {
            _userFeedbacks.Insert(ufb);
            _uow.SavaChange();
            return true;
        }

        public IEnumerable<Products> ProductsWhere(Expression<Func<Products, bool>> Filter = null)
        {
            if (Filter != null)
            {
                return _products.Where(Filter).Take(200);
            }
            return null;
        }

    }


}
