// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

namespace WebTrendsSDK.Api;

/// <summary>
/// Provides factory methods for creating a WebTrends API client.
/// </summary>
public interface IWebTrendsApiClientFactory
{
	/// <summary>
	/// Creates a WebTrends API client.
	/// </summary>
	/// <returns>The API settings.</returns>
	IWebTrendsApiClient CreateApiClient(WebTrendsSettings settings);
}

public class WebTrendsApiClientFactory(IWebTrendsHttpClientFactory httpClientFactory) : IWebTrendsApiClientFactory
{
	readonly IWebTrendsHttpClientFactory _httpClientFactory
		= Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

	public IWebTrendsApiClient CreateApiClient(WebTrendsSettings settings)
	{
		Ensure.IsNotNull(settings, nameof(settings));

		var http = _httpClientFactory.CreateHttpClient("WebTrends");

		return new WebTrendsApiClient(http, settings);
	}
}
