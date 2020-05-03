﻿using CoursePlus.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoursePlus.Client
{
    public class InputSelectCustom<T> : InputSelect<T>
    {
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
            if (typeof(T) == typeof(EnumLanguages) ||
                typeof(T) == typeof(EnumLanguages?))
            {
                if (Enum.TryParse<EnumLanguages>(value, out var resultEnum))
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
    }
}
