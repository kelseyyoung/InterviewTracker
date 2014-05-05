using Elfar;
using Elfar.Xml;
using InterviewTracker.Filters;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InterviewTracker
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            var provider = new XmlErrorLogProvider();
            GlobalFilters.Filters.Add(new ErrorLogFilter(provider));
            RouteTable.Routes.Insert(0, new ErrorLogRoute(provider));
        }
    }
}