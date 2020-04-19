using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using WebApiPack.ConstantValues;

namespace Microsoft.AspNetCore.Builder {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseDefaultBuilder(this IApplicationBuilder app, string environmentName) {
            static IEnvironmentBuilder GetEnvironmentBuilder(string environmentName) {
                IEnvironmentBuilder environmentBuilder;
                environmentBuilder = environmentName switch
                {
                    DefaultEnvironmentNames.Development => new DevelopmentBuilder(),
                    DefaultEnvironmentNames.DevelopmentRemote => new DevelopmentRemoteBuilder(),
                    DefaultEnvironmentNames.Staging => new StagingBuilder(),
                    DefaultEnvironmentNames.Production => new ProductionBuilder(),
                    _ => throw new System.ArgumentException($"{nameof(environmentName)}")
                };

                return environmentBuilder;
            }

            return GetEnvironmentBuilder(environmentName)
                .UseEnvironmentBuilder(app)
                .UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto })
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(CorsConstantValues.PolicyName)
                .UseAuthorization()
                .UseExceptionMiddleware();
        }

        private interface IEnvironmentBuilder {
            IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app);
        }

        private class DevelopmentRemoteBuilder : IEnvironmentBuilder {
            public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) => app.UseDeveloperExceptionPage();
        }

        private class DevelopmentBuilder : IEnvironmentBuilder {
            public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) => app.UseDeveloperExceptionPage();
        }

        private class StagingBuilder : IEnvironmentBuilder {
            public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) => app.UseHsts();
        }

        private class ProductionBuilder : IEnvironmentBuilder {
            public IApplicationBuilder UseEnvironmentBuilder(IApplicationBuilder app) => app.UseHsts();
        }
    }
}
