// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

using System.Net.Http.Headers;

using WebTrendsSDK;
using WebTrendsSDK.Api;

using Microsoft.Extensions.Configuration;

var settings = GetSettings();
var http = CreateHttpClient();
var api = new WebTrendsApiClient(http, settings);

const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36 Edg/124.0.0.0";

var project = await api.Ots.Control.GetProjectAsync("ta_1614ReducedCheckout", userAgent: UserAgent);

var projects = await api.Ots.Control.GetProjectsAsync(userAgent: UserAgent);

Console.WriteLine(project);

WebTrendsSettings GetSettings()
{
	var configuration = new ConfigurationBuilder()
		.AddJsonFile("./appsettings.json", optional: false)
		.AddJsonFile("./appsettings.env.json", optional: true)
		.Build();

	WebTrendsSettings settings = new();
	configuration.GetSection(WebTrendsSettings.ConfigurationSection).Bind(settings);

	settings.Validate();

	return settings;
}

HttpClient CreateHttpClient()
{
	var http = new HttpClient();

	http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

	return http;
}
