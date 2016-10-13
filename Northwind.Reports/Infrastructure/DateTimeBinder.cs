using System;
using System.Globalization;
using System.Web.Mvc;

namespace Northwind.Web
{
    public class DateTimeBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            if (controllerContext == null) throw new ArgumentNullException(nameof(controllerContext));
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value == null) throw new ArgumentNullException(bindingContext.ModelName);

            var cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);

            try {
                return value.ConvertTo(typeof(DateTime), cultureInfo);
            }
            catch (Exception ex) {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
                return null;
            }
        }

    }
}