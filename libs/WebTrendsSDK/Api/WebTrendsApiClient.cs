// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

namespace WebTrendsSDK.Api;

public partial interface IWebTrendsApiClient
{

}

public partial class WebTrendsApiClient(HttpClient http, WebTrendsSettings settings)
	: ApiClient(http, settings), IWebTrendsApiClient
{ }
