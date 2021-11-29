using Microsoft.Practices.Unity;
using Pos.Domain.UnitOfWork;
using System.Web.Mvc;
using Unity.Mvc5;
using Pos.Domain.Repository;
using Pos.Services.Service;
using Pos.Services.Interfaces;
using Pos.DataObjects.EntityFramework;
using Pos.Services.Services;

namespace Pos.IoC
{
    public static class UnitiyConfig
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
            container.BindInRequestScope<ICashRegisterService, CashRegisterService>();
            container.BindInRequestScope<IUserService, UserService>();

            container.BindInRequestScope<IGRepository<T_CASH_REGISTER>, GRepository<T_CASH_REGISTER>>();
            container.BindInRequestScope<IGRepository<T_USERS>, GRepository<T_USERS>>();
            
        }
        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }
    }
}


