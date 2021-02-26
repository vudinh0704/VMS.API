using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace VMS.Api.Helpers
{
    public class CustomBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return null;
        }
    }
}