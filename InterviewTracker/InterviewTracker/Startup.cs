using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InterviewTracker.Startup))]
namespace InterviewTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
