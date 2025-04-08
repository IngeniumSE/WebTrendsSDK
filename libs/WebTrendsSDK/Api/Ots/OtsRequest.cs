// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

using System.Text.Json.Serialization;

namespace WebTrendsSDK.Api;

internal record OtsRequest(
	string url,
	[property: JsonPropertyName("s_mode")] string state);
