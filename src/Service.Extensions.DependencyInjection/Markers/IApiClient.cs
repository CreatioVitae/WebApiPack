using System.Net.Http;

namespace Service.Extensions.DependencyInjection.Markers;

public interface IApiClient {
    HttpClient HttpClient { get; }
}
