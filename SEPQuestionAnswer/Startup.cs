using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SEPQuestionAnswer.Startup))]
namespace SEPQuestionAnswer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
