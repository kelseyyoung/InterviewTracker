﻿using InterviewTracker.Filters;
using System.Web;
using System.Web.Mvc;

namespace InterviewTracker
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}