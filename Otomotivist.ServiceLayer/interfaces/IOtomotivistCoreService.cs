using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Otomotivist.Service.interfaces
{
    public interface IOtomotivistCoreService
    {
        IQueryable<Products> NewAutomobilesTop30();
        IQueryable<Products> NewSparepartsTop30();
        IQueryable<ProductGroups> GetProductroupById(int id);
        IQueryable<Products> GetProductsByType(int productType);
        IQueryable<Products> GetProductsByTypeAndContainerCategory(int productType, List<int?> parentTypeIds);
        IQueryable<Products> GetProductsByTypeAndCriteriaCode(int productType, string criteriaCode);
        IQueryable<Products> GetProductsByTypeAndCriteriaCodeAndContainer(int productType, string criteriaCode, List<int?> containerIds);
        Products GetProductsById(int pid);
        IQueryable<Categories> GetCategoriesById(int parentId);
        IQueryable<Categories> GetCategoriesById(Int64 id);
        IQueryable<Categories> GetCategoriesByIdIfProductExist(int id);
        Categories GetCategoriesByCategoryId(int categoryId);
        IQueryable<Categories> GetCategoriesByContainer(List<int?> containerIds);
        IQueryable<Towns> GetTownsById(int id);
        IQueryable<SubDistrict> GetSubdistrictsById(int id);
        UserProfile GetUserProfileById(int userid);
        UserProfile GetUserProfileByName(string username);
        IQueryable<UserAdresses> GetUserAddressesByUserId(int addressId);
        IQueryable<UserAdresses> GetUserAddressesById(int userid);
        IQueryable<UserFeedbacks> GetUserUserFeedbacksById(int userid);
        IQueryable<UserMessages> GetUserMessagesById(int userid);
        IQueryable<UserEducationLevel> GetUserEducationLevelsById(int? userid);
        IQueryable<UserJob> GetUserJobsById(int? jid);
        IQueryable<UserGender> GetUserGenders();
        IQueryable<UserEducationLevel> GetUserEducationLevels();
        IQueryable<UserJob> GetUserJobs();
        IQueryable<AutoServices> Getautoservices(int registererId);
        IQueryable<Galleries> GetGalleries(int userId);
        IQueryable<Orders> GetOrders(int RequesterUserId);
        IQueryable<Products> GetProducts(int RequesterUserId);
        IQueryable<ProductGroups> GetProductGroups();
        IQueryable<Categories> GetCategories();
        IQueryable<Marks> GetMarks();
        IQueryable<Currencies> GetCurrencies();
        IQueryable<ProductGroups> GetProductGroupsAll();
        ProductGroups GetProductGroupsById(int productGroupId);
        IQueryable<Categories> GetCategoriesAll();
        IQueryable<Marks> GetMarksAll();
        IQueryable<Denominations> GetDenominationsAll();
        IQueryable<ProductStates> GetProductStatesAll();
        IQueryable<Currencies> GetCurrenciesAll();
        Currencies GetCurrenciesById(int currId);
        IQueryable<Cities> GetCitiesAll();
        Cities GetCitiesById(int id);
        IQueryable<CaseTypes> GetCaseTypesAll();
        CaseTypes GetCaseTypesById(int caseTypeId);
        IQueryable<Colors> GetColorsAll();
        Colors GetColorsById(int colorId);
        IQueryable<DamageStates> GetDamageStatesAll();
        IQueryable<FuelTypes> GetFuelTypesAll();
        FuelTypes GetFuelTypesById(int fuelTypeId);
        IQueryable<ModelYears> GetModelYearsAll();
        IQueryable<EnginePowers> GetEnginePowersAll();
        EnginePowers GetEnginePowersById(int enginePowerId);
        IQueryable<EngineVolumes> GetEngineVolumesAll();
        EngineVolumes GetEngineVolumesById(int engineVolumeId);
        IQueryable<GearTypes> GetGearTypesAll();
        GearTypes GetGearTypesById(int gearTypeId);
        IQueryable<VehicleTypes> GetVehicleTypesAll();
        VehicleTypes GetVehicleTypesById(int vehicleTypeId);
        IQueryable<PlateNationalities> GetPlateNationalitiesAll();
        IQueryable<TractionTypes> GetTractionTypesAll();
        IQueryable<TransportTypes> GetTransportTypesAll();
        IQueryable<GuarantySituations> GetGuarantySituationsAll();
        IQueryable<PublishDurations> GetPublishDurationsAll();
        IQueryable<ProductSeller> GetProductSellersAll();
        ProductSeller GetProductSellersById(int productSellerId);
        IQueryable<Products> GetProductByCriteria(string criteria);
        Cities GetCityById(int id);
        string[] FindCategories(int? id);

        bool InsertProduct(Products prd);
        bool InsertMessage(UserMessages msg);
        bool InsertFeedBacks(UserFeedbacks ufb);

        IEnumerable<Products> ProductsWhere(Expression<Func<Products, bool>> Filter = null);

    }
}
