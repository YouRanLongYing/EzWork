﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Ez.UI.Validations;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ez.UI.HtmlExtends.FormAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HiddenUIAttribute : PropertyUIAttribute
    {
        public override TagType UIType
        {
            get
            {
                return TagType.Hidden;
            }
        }

        public override RouteValueDictionary GetAttributes()
        {
            return base.GetAttributes();
        }
    }
}
