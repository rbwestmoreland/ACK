using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using TinyIoC;

namespace Ack.Web.Controllers.WebApi.DependencyResolvers
{
    public class TinyIoCDependencyResolver : IDependencyResolver
    {
        private TinyIoCContainer Container { get; set; }

        public TinyIoCDependencyResolver(TinyIoCContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Container = container;
        }

        public IDependencyScope BeginScope()
        {
            var childContainer = Container.GetChildContainer();
            return new TinyIoCDependencyResolver(childContainer);
        }

        public object GetService(Type serviceType)
        {
            object resolvedType = null;
            Container.TryResolve(serviceType, out resolvedType);
            return resolvedType;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var resolvedTypes = Container.ResolveAll(serviceType);
            return resolvedTypes;
        }

        #region Disposable Member(s)

        private bool Disposed { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                    Container.Dispose();
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                Disposed = true;
            }
        }

        #endregion Disposable Member(s)
    }
}