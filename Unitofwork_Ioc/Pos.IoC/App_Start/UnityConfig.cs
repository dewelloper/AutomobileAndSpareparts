using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Otomotivist.Domain.UnitOfWork;
using Otomotivist.Domain.Repository;
using Otomotivist.Service.interfaces;
using Otomotivist.Service.services;
using Dal;


namespace Otomotivist.IoC
{
    public static class UnityConfig
    {

        public static void RegisterComponents()
        {
            UnityContainer container = new UnityContainer();
            RegisterTypes(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.BindInRequestScope<IUnitOfWork, UnitOfWork>();
            container.BindInRequestScope<IOtomotivistCoreService, OtomotivistCoreService>();
            container.BindInRequestScope<IAccountService, AccountService>();
            container.BindInRequestScope<IOrderService, OrderService>();
            //container.BindInRequestScope<ExtendedMembershipProvider, SimpleMembershipProvider>();
            

            container.BindInRequestScope<IGRepository<Products>, GRepository<Products>>();
            container.BindInRequestScope<IGRepository<ProductGroups>, GRepository<ProductGroups>>();
            container.BindInRequestScope<IGRepository<Categories>, GRepository<Categories>>();
            container.BindInRequestScope<IGRepository<Towns>, GRepository<Towns>>();
            container.BindInRequestScope<IGRepository<SubDistrict>, GRepository<SubDistrict>>();
            container.BindInRequestScope<IGRepository<UserProfile>, GRepository<UserProfile>>();
            container.BindInRequestScope<IGRepository<UserAdresses>, GRepository<UserAdresses>>();
            container.BindInRequestScope<IGRepository<UserFeedbacks>, GRepository<UserFeedbacks>>();
            container.BindInRequestScope<IGRepository<UserMessages>, GRepository<UserMessages>>();
            container.BindInRequestScope<IGRepository<UserEducationLevel>, GRepository<UserEducationLevel>>();
            container.BindInRequestScope<IGRepository<UserJob>, GRepository<UserJob>>();
            container.BindInRequestScope<IGRepository<UserGender>, GRepository<UserGender>>();
            container.BindInRequestScope<IGRepository<AutoServices>, GRepository<AutoServices>>();
            container.BindInRequestScope<IGRepository<Galleries>, GRepository<Galleries>>();
            container.BindInRequestScope<IGRepository<Orders>, GRepository<Orders>>();
            container.BindInRequestScope<IGRepository<Marks>, GRepository<Marks>>();
            container.BindInRequestScope<IGRepository<Currencies>, GRepository<Currencies>>();
            container.BindInRequestScope<IGRepository<Denominations>, GRepository<Denominations>>();
            container.BindInRequestScope<IGRepository<ProductStates>, GRepository<ProductStates>>();
            container.BindInRequestScope<IGRepository<Cities>, GRepository<Cities>>();
            container.BindInRequestScope<IGRepository<CaseTypes>, GRepository<CaseTypes>>();
            container.BindInRequestScope<IGRepository<Colors>, GRepository<Colors>>();
            container.BindInRequestScope<IGRepository<DamageStates>, GRepository<DamageStates>>();
            container.BindInRequestScope<IGRepository<FuelTypes>, GRepository<FuelTypes>>();
            container.BindInRequestScope<IGRepository<ModelYears>, GRepository<ModelYears>>();
            container.BindInRequestScope<IGRepository<EnginePowers>, GRepository<EnginePowers>>();
            container.BindInRequestScope<IGRepository<EngineVolumes>, GRepository<EngineVolumes>>();
            container.BindInRequestScope<IGRepository<GearTypes>, GRepository<GearTypes>>();
            container.BindInRequestScope<IGRepository<VehicleTypes>, GRepository<VehicleTypes>>();
            container.BindInRequestScope<IGRepository<PlateNationalities>, GRepository<PlateNationalities>>();
            container.BindInRequestScope<IGRepository<TractionTypes>, GRepository<TractionTypes>>();
            container.BindInRequestScope<IGRepository<GuarantySituations>, GRepository<GuarantySituations>>();
            container.BindInRequestScope<IGRepository<PublishDurations>, GRepository<PublishDurations>>();
            container.BindInRequestScope<IGRepository<ProductSeller>, GRepository<ProductSeller>>();
            container.BindInRequestScope<IGRepository<webpages_Membership>, GRepository<webpages_Membership>>();
            container.BindInRequestScope<IGRepository<Bills>, GRepository<Bills>>();
            container.BindInRequestScope<IGRepository<OrderDetails>, GRepository<OrderDetails>>();

        }

        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }


    }
}