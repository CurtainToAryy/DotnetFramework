using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Mvc.Controllers
{
    public class ServiceBasedControllerActivator : IControllerActivator
    {
        public object Create(ControllerContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            var controllerType = actionContext.ActionDescriptor.ControllerTypeInfo.AsType();
            return actionContext.HttpContext.RequestServices.GetRequiredService(controllerType);
            //return ServiceLocator.Current.GetService(controllerType);
        }

        public virtual void Release(ControllerContext context, object controller)
        {
            var disposable = controller as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
