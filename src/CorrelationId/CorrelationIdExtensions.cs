using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CorrelationId
{
	/// <summary>
	/// Extension methods for the CorrelationIdMiddleware.
	/// </summary>
	public static class CorrelationIdExtensions
	{
		/// <summary>
		/// Enables correlation IDs for the request.
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
		public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			return app.UseCorrelationId(new CorrelationIdOptions());
		}

		/// <summary>
		/// Enables correlation IDs for the request.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="header">The header field name to use for the correlation ID.</param>
		/// <returns></returns>
		public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, string header)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			return app.UseCorrelationId(new CorrelationIdOptions
			{
				Header = header
			});
		}

		/// <summary>
		/// Enables correlation IDs for the request.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, CorrelationIdOptions options)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			if (app.ApplicationServices.GetService(typeof(ICorrelationContextFactory)) == null)
			{
				throw new InvalidOperationException(
					"Unable to find the required services. You must call the AddCorrelationId method in ConfigureServices in the application startup code.");
			}

			return app.UseMiddleware<CorrelationIdMiddleware>(Options.Create(options));
		}

		/// <summary>
		/// Returns CorrelationId from HttpContext.Items collection
		/// </summary>
		/// <param name="context"></param>
		/// <param name="itemName">Name of the Item containing CorrelationId</param>
		/// <returns>CorrelationId or empty string</returns>
		public static Guid GetCorrelationId(this HttpContext context, string itemName)
		{
			return context.Items.ContainsKey(itemName)
				? (Guid) context.Items[itemName]
				: Guid.Empty;
		}
	}
}