namespace AccountService.Api.Tests.Infrastructure
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Api.Infrastructure;
    using Api.Infrastructure.Configuration;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Swashbuckle.AspNetCore.Swagger;

    public class TestStartup
    {
        private const string DefaultCulture = "en-GB";
        private const string SupportedCultures = "en-GB;en-US;en;nl-BE;nl;fr-BE;fr";

        private IContainer _applicationContainer;

        private readonly IConfiguration _configuration;

        public TestStartup(
            IConfiguration configuration) =>
            _configuration = configuration;

        /// <summary>Configures services for the application.</summary>
        /// <param name="services">The collection of services to configure the application with.</param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureDefaultForApi<Startup, SharedResources>(new StartupConfigureOptions
                {
                    Cors =
                    {
                        Origins = _configuration
                            .GetSection("Cors")
                            .GetChildren()
                            .Select(c => c.Value)
                            .ToArray()
                    },
                    Swagger =
                    {
                        ApiInfo = (provider, description) => new Info
                        {
                            Version = description.ApiVersion.ToString(),
                            Title = "Account API",
                            Description = GetApiLeadingText(description),
                            Contact = new Contact
                            {
                                Name = "exira.com",
                                Email = "info@exira.com",
                                Url = "https://exira.com"
                            }
                        },
                        XmlCommentPaths = new [] { typeof(Startup).GetTypeInfo().Assembly.GetName().Name }
                    },
                    Localization =
                    {
                        DefaultCulture = new CultureInfo(DefaultCulture),
                        SupportedCultures = SupportedCultures
                            .Split(';', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => new CultureInfo(x.Trim()))
                            .ToArray()
                    },
                    MiddlewareHooks =
                    {
                        FluentValidation = fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>()
                    }
                });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            _applicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(_applicationContainer);
        }

        public void Configure(
            IServiceProvider serviceProvider,
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime appLifetime,
            ILoggerFactory loggerFactory,
            IApiVersionDescriptionProvider apiVersionProvider)
        {
            app.UseDefaultForApi(new StartupUseOptions
            {
                Common =
                {
                    ApplicationContainer = _applicationContainer,
                    ServiceProvider = serviceProvider,
                    HostingEnvironment = env,
                    ApplicationLifetime = appLifetime,
                    LoggerFactory = loggerFactory,
                },
                Api =
                {
                    VersionProvider = apiVersionProvider,
                    Info = groupName => $"exira.com - Account API {groupName}",
                },
                Server =
                {
                    PoweredByName = "exira.com",
                    ServerName = "exira.com"
                },
                MiddlewareHooks =
                {
                    AfterMiddleware = x => x.UseMiddleware<AddNoCacheHeadersMiddleware>(),
                },
            });
        }

        private static string GetApiLeadingText(ApiVersionDescription description)
            => $"Right now you are reading the documentation for version {description.ApiVersion} of the exira.com - Account API{string.Format(description.IsDeprecated ? ", **this API version is not supported any more**." : ".")}";
    }
}
