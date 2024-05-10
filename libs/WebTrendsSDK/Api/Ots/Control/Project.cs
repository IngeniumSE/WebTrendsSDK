// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

using System.Text.Json.Serialization;

using WebTrendsSDK.Api;

namespace WebTrendsSDK;

/// <summary>
/// Represents a WebTrends project.
/// </summary>
public class Project : OtsResponse<ProjectBody>
{
	[JsonPropertyName("opcode")]
	public string? OpCode { get; set; }

	[JsonPropertyName("opstatus")]
	public string? OpsStatus { get; set; }

	[JsonPropertyName("guid")]
	public string? Guid { get; set; }
}

public class ProjectBody : Body
{
	[JsonPropertyName("factors")]
	public ProjectFactor[]? Factors { get; set; }
}

public class ProjectFactor
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = default!;

	[JsonPropertyName("operation")]
	public int Operation { get; set; }

	[JsonPropertyName("value")]
	public string? Value { get; set; }
}
