// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

using System.Net.Http.Headers;

using WebTrendsSDK;
using WebTrendsSDK.Api;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extensions for the <see cref="IServiceCollection"/> type.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds Sailthru services to the given services collection.
	/// </summary>
	/// <param name="services">The services collection.</param>
	/// <param name="configure">The configure delegate.</param>
	/// <returns>The services collection.</returns>
	public static IServiceCollection AddWebTrends(
		this IServiceCollection services,
		Action<WebTrendsSettings> configure)
	{
		Ensure.IsNotNull(services, nameof(services));
		Ensure.IsNotNull(configure, nameof(configure));

		services.Configure(configure);

		AddCoreServices(services);

		return services;
	}

	/// <summary>
	/// Adds Sailthru services to the given services collection.
	/// </summary>
	/// <param name="services">The services collection.</param>
	/// <param name="settings">The Sailthru settings.</param>
	/// <returns>The services collection.</returns>
	public static IServiceCollection AddWebTrends(
		this IServiceCollection services,
		WebTrendsSettings settings)
	{
		Ensure.IsNotNull(services, nameof(services));
		Ensure.IsNotNull(settings, nameof(settings));

		services.AddSingleton(settings.AsOptions());

		AddCoreServices(services);

		return services;
	}

	/// <summary>
	/// Adds Sailthru services to the given services collection.
	/// </summary>
	/// <param name="services">The services collection.</param>
	/// <param name="configuration">The configuration.</param>
	/// <returns>The services collection.</returns>
	public static IServiceCollection AddWebTrends(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		Ensure.IsNotNull(services, nameof(services));
		Ensure.IsNotNull(configuration, nameof(configuration));

		services.Configure<WebTrendsSettings>(
			configuration.GetSection(WebTrendsSettings.ConfigurationSection));

		AddCoreServices(services);

		return services;
	}

	static void AddCoreServices(IServiceCollection services)
	{
		void ConfigureHttpDefaults(HttpClient http)
		{
			http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		services.AddSingleton(sp =>
		{
			var settings = sp.GetRequiredService<IOptions<WebTrendsSettings>>().Value;

			settings.Validate();

			return settings;
		});

		services.AddHttpClient("WebTrends", ConfigureHttpDefaults);
		services.AddScoped<IWebTrendsHttpClientFactory, WebTrendsHttpClientFactory>();
		services.AddScoped<IWebTrendsApiClientFactory, WebTrendsApiClientFactory>();

		services.AddScoped(sp =>
		{
			var settings = sp.GetRequiredService<WebTrendsSettings>();
			var factory = sp.GetRequiredService<IWebTrendsApiClientFactory>();

			return factory.CreateApiClient(settings);
		});
	}
}
