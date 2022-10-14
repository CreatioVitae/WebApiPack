using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;
using WebApiPack.ConstantValues;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

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

        if (configureSettings?.PathBase is not null) {
            app.UsePathBase(configureSettings.PathBase.Value);
        }

        GetEnvironmentBuilder(environmentName).UseEnvironmentBuilder(app);

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors(CorsConst.PolicyName);

        if (configureSettings?.AuthenticationEnable ?? false) {
            app.UseAuthentication();
        }

        app.UseAuthorization();

        return app.UseExceptionMiddleware();
    }

    interface IEnvironmentBuilder {
        IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app);
    }

    class DevelopmentRemoteBuilder : IEnvironmentBuilder {
        public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) =>
            app.UseDeveloperExceptionPage();
    }

    class DevelopmentBuilder : IEnvironmentBuilder {
        public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) =>
            app.UseDeveloperExceptionPage();
    }

    class StagingBuilder : IEnvironmentBuilder {
        public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) =>
            app;
    }

    class ProductionBuilder : IEnvironmentBuilder {
        public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) =>
            app;
    }
}
