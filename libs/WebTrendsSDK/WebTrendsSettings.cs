// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

using FluentValidation;

using Microsoft.Extensions.Options;

namespace WebTrendsSDK;

/// <summary>
/// Represents the settings for the WebTrends API.
/// </summary>
public class WebTrendsSettings
{
	public const string ConfigurationSection = "WebTrends";

	/// <summary>
	/// Gets the account ID.
	/// </summary>
	public string AccountId { get; set; } = default!;

	/// <summary>
	/// Gets or sets whether to capture request content.
	/// </summary>
	public bool CaptureRequestContent { get; set; }

	/// <summary>
	/// Gets or sets whether to capture response content.
	/// </summary>
	public bool CaptureResponseContent { get; set; }

	/// <summary>
	/// Gets or sets whether to request debug logging with OTS responses.
	/// </summary>
	public bool Debug { get; set; } = false;

	/// <summary>
	/// Gets or sets whether to request encrypted cookie values.
	/// </summary>
	public bool Encrypted { get; set; } = true;

	/// <summary>
	/// Gets or sets the authentication token.
	/// </summary>
	public string KeyToken { get; set; } = default!;

	/// <summary>
	/// Gets or sets the environment for the WebTrends OTS API.
	/// </summary>
	public string OtsBaseUrl { get; set; } = "https://ots.webtrends-optimize.com";

	/// <summary>
	/// Gets or sets the default state for the WebTrends API.
	/// </summary>
	public State State { get; set; } = State.Normal;

	/// <summary>
	/// Gets or sets whether to track the returned content as a View in WebTrends.
	/// </summary>
	public bool Track { get; set; } = true;

	/// <summary>
	/// Gets or sets the default website URL.
	/// </summary>
	public string WebsiteUrl { get; set; } = default!;

	/// <summary>
	/// Returns the settings as a wrapped options instance.
	/// </summary>
	/// <returns>The options instance.</returns>
	public IOptions<WebTrendsSettings> AsOptions()
		=> Options.Create(this);

	/// <summary>
	/// Validates the current instance.
	/// </summary>
	public void Validate()
		=> WebTrendsSettingsValidator.Instance.Validate(this);
}

/// <summary>
/// Validates the settings for the WebTrends API.
/// </summary>
public class WebTrendsSettingsValidator : AbstractValidator<WebTrendsSettings>
{
	public static readonly WebTrendsSettingsValidator Instance = new();

	public WebTrendsSettingsValidator()
	{
		RuleFor(x => x.AccountId).NotEmpty();
		RuleFor(x => x.OtsBaseUrl).NotEmpty();
		RuleFor(x => x.KeyToken).NotEmpty();
		RuleFor(x => x.WebsiteUrl).NotEmpty();
	}
}
