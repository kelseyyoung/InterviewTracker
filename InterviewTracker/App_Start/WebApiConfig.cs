﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using InterviewTracker.Filters;

namespace InterviewTracker
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Adds the filter to report back model errors
            config.Filters.Add(new ValidateModelAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = @"^[0-9]*$" }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            /*
             * Removes XML formatting and gets rid of circular object references
             */
           
            var json = config.Formatters.JsonFormatter;
            /*
            json.SerializerSettings.PreserveReferencesHandling =
                Newtonsoft.Json.PreserveReferencesHandling.Objects;
             */
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; 

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Enable querying support for OData globally
            //config.EnableQuerySupport();
        }
    }
}