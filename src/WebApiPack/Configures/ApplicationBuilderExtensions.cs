using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using WebApiPack.Configurations;
using WebApiPack.ConstantValues;

namespace Microsoft.AspNetCore.Builder {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseDefaultBuilder(this IApplicationBuilder app, string environmentName, ConfigureSettings? configureSettings = null) {
            static IEnvironmentBuilder GetEnvironmentBuilder(string environmentName) =>
                environmentName switch {
                    DefaultEnvironmentNames.Development => new DevelopmentBuilder(),
                    DefaultEnvironmentNames.DevelopmentRemote => new DevelopmentRemoteBuilder(),
                    DefaultEnvironmentNames.Staging => new StagingBuilder(),
                    DefaultEnvironmentNames.Production => new ProductionBuilder(),
                    _ => new DevelopmentBuilder(),
                };

            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

            if (configureSettings?.PathBase is PathBase) {
                app.UsePathBase(configureSettings.PathBase.Value);
            }

            GetEnvironmentBuilder(environmentName).UseEnvironmentBuilder(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CorsConst.PolicyName);

            if (configureSettings?.AuthenticationIsEnable ?? false) {
                app.UseAuthentication();
            }

            app.UseAuthorization();

            return app.UseExceptionMiddleware();
        }

        public static ConfigureSettings? GetConfigureSettings(this IConfiguration configuration) =>
            configuration.GetSection(nameof(ConfigureSettings)).Get<ConfigureSettings>();

        private interface IEnvironmentBuilder {
            IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app);
        }

        private class DevelopmentRemoteBuilder : IEnvironmentBuilder {
            public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) =>
                app.UseDeveloperExceptionPage();
        }

        private class DevelopmentBuilder : IEnvironmentBuilder {
            public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) =>
                app.UseDeveloperExceptionPage();
        }

        private class StagingBuilder : IEnvironmentBuilder {
            public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) =>
                app;
        }

        private class ProductionBuilder : IEnvironmentBuilder {
            public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) =>
                app;
        }
    }
}
