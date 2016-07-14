using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repro.Hubs;

namespace Repro
{
    public class Startup : IDisposable
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // this property is static only for testing purposes!!
        public static bool IsDisposed { get; set; }

        private IContainer Container { get; set; }

        public void Dispose()
        {
            Container.Dispose();

            IsDisposed = true;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSignalR(opt =>
            {
                opt.Hubs.EnableDetailedErrors = true;
            });

            var builder = new ContainerBuilder();
            builder.RegisterType<SomeClassThatHasRequiresSignalR>();
            builder.Populate(services);
            Container = builder.Build();

            return Container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseWebSockets();

            app.UseSignalR();

            appLifetime.ApplicationStopped.Register(Dispose);
        }
    }

    public class SomeClassThatHasRequiresSignalR
    {
        public SomeClassThatHasRequiresSignalR(IHubContext<SomeHub> hubContext)
        {
            HubContext = hubContext;
        }

        private IHubContext<SomeHub> HubContext { get; }
    }
}
