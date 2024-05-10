// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

namespace WebTrendsSDK.Api;

public partial interface IWebTrendsApiClient
{

}

public partial class WebTrendsApiClient : ApiClient, IWebTrendsApiClient
{
	public WebTrendsApiClient(HttpClient http, WebTrendsSettings settings)
		: base(http, settings) { }
}
