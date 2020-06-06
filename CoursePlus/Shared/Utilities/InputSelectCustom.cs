using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoursePlus.Shared.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace CoursePlus.Shared.Utilities
{
    public class InputSelectCustom<T> : InputSelect<T>
    {
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        protected override string FormatValueAsString(T value)
        {
            if (CssClass.Contains("selectpicker"))
            {
                var id = "#" + AdditionalAttributes.First(x => x.Key == "id").Value;
                JsRuntime.InvokeAsync<object>("selectpicker", id, value.ToString());
            }
            return base.FormatValueAsString(value);
        }

        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage) 
        {
            if (typeof(T) == typeof(int) ||
                typeof(T) == typeof(int?))
            {
                if (int.TryParse(value, out var resultInt))
                {
                    result = (T)(object)resultInt;
                    validationErrorMessage = null;
                    return true;
                }
                else
                {
                    result = default;
                    validationErrorMessage = "The chosen value is not valid.";
                    return false;
                }
            }
            else 
            if (typeof(T).IsEnum)
            {
                if (CustomFunctions.EnumTryParse<T>(value, out var resultEnum))
                {
                    result = (T)(object)resultEnum;
                    validationErrorMessage = null;
                    return true;
                }
                else
                {
                    result = default;
                    validationErrorMessage = "The chosen value is not valid.";
                    return false;
                }
            }
            else
            if (typeof(T).IsNullableEnum())
            {
                if (CustomFunctions.NullableEnumTryParse<T>(value, out var resultEnum))
                {
                    result = (T)(object)resultEnum;
                    validationErrorMessage = null;
                    return true;
                }
                else
                {
                    result = default;
                    validationErrorMessage = "The chosen value is not valid.";
                    return false;
                }
            }
            else
            {
                return base.TryParseValueFromString(value, out result, out validationErrorMessage);
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender) FormatValueAsString(Value);
            base.OnAfterRender(firstRender);
        }
    }
}
