using System.Web.Mvc;
using System.Web.Routing;
using Elfar;
using Elfar.Xml;

[assembly: WebActivator.PreApplicationStartMethod(typeof(InterviewTracker.App_Start.Elfar), "Init")]
namespace InterviewTracker.App_Start
{
    public static class Elfar
    {
        public static void Init()
        {
            var provider = new XmlErrorLogProvider();
            GlobalFilters.Filters.Add(new ErrorLogFilter(provider));
            RouteTable.Routes.Insert(0, new ErrorLogRoute(provider));
        }
    }
}