using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace EPrescribing.Web.Helpers
{
    public class DateTimeBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string actionName = controllerContext.RouteData.Values["action"].ToString();
            string controllerName = controllerContext.RouteData.Values["controller"].ToString();
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            string val = "";
            if (value != null)
            {
                val = value.AttemptedValue;
                DateTime date;
                var displayFormat = "dd/MM/yyyy";
                if (value.AttemptedValue.Contains(","))
                {
                    val = value.AttemptedValue.Split(',')[1];
                }
                if (val != "")
                {
                    var count = val.Count();

                    if (val.Count() != 5)
                    {
                        if (count > 10)
                        {
                            displayFormat = "dd/MM/yyyy hh:mm tt";
                            if (DateTime.TryParseExact(val, displayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                            {
                                return date;
                            }
                            else
                            {
                                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid Date Format");
                            }
                        }
                        else
                        {
                            if (DateTime.TryParseExact(val, displayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                            {
                                return date;
                            }
                            else
                            {
                                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid Date Format");
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.TryParseExact(val, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            return date;
                        }
                        else
                        {
                            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid Time Format");
                        }
                    }
                }
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
