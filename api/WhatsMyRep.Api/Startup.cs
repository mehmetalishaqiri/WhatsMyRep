/*   
    The MIT License (MIT)
    
    Copyright (C) 2011 Mehmetali Shaqiri (mehmetalishaqiri@gmail.com)
 
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE. 
 */

using System.Reflection;
using System.Web.Http;
using Microsoft.Owin;
using Newtonsoft.Json;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using WhatsMyRep.Api;
using WhatsMyRep.Api.Core.Json;
using WhatsMyRep.Api.Core.Tfs;

[assembly: OwinStartup(typeof(Startup))]

namespace WhatsMyRep.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.

            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(config);

            // make sure browsers get JSON without compromising content negotiation from clients that actually want XML.
            config.Formatters.Add(new JsonFormatter()
            {
                SerializerSettings = { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
            });
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            RegisterServices(kernel);

            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ITfsService>().To<TfsService>();
        }
    }
}
