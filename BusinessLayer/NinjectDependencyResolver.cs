using BusinessLayer.Implementation;
using BusinessLayer.Interface;
using DataAccess.Implementation;
using DataAccess.Interface;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessLayer
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // BIND INTERFACE AND IMPLEMENTATION OF BUSNESS
            kernel.Bind<IUserBusiness>().To<UserBusiness>();
            kernel.Bind<ITransactionsBusiness>().To<TransactionsBusiness>();

            // BIND INTERFACE AND IMPLEMENTATION OF DATA ACCESS
            kernel.Bind<IUserDataAccess>().To<UserDataAccess>();
            kernel.Bind<ITransactionsDataAccess>().To<TransactionsDataAccess>();
        }
    }
}
