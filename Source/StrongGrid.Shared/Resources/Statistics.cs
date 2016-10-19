﻿using Newtonsoft.Json.Linq;
using StrongGrid.Model;
using StrongGrid.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace StrongGrid.Resources
{
	public class Statistics
	{
		private readonly string _endpoint;
		private readonly IClient _client;

		/// <summary>
		/// Constructs the SendGrid Statistics object.
		/// See https://sendgrid.com/docs/API_Reference/Web_API_v3/Stats/index.html
		/// </summary>
		/// <param name="client">SendGrid Web API v3 client</param>
		/// <param name="endpoint">Resource endpoint, do not prepend slash</param>
		public Statistics(IClient client, string endpoint = "/stats")
		{
			_endpoint = endpoint;
			_client = client;
		}

		/// <summary>
		/// Get all global email statistics for a given date range.
		/// See: https://sendgrid.com/docs/API_Reference/Web_API_v3/Stats/global.html
		/// </summary>
		/// <param name="startDate">The starting date of the statistics to retrieve.</param>
		/// <param name="endDate">The end date of the statistics to retrieve. Defaults to today.</param>
		/// <param name="aggregatedBy">How to group the statistics, must be day|week|month</param>
		/// <returns></returns>
		public async Task<Statistic[]> GetGlobalStatisticsAsync(DateTime startDate, DateTime? endDate = null, AggregateBy aggregatedBy = AggregateBy.None, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["start_date"] = startDate.ToString("yyyy-MM-dd");
			if (endDate.HasValue) query["end_date"] = endDate.Value.ToString("yyyy-MM-dd");
			if (aggregatedBy != AggregateBy.None) query["aggregated_by"] = aggregatedBy.GetDescription();

			var response = await _client.GetAsync(string.Format("{0}?{1}", _endpoint, query), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var statistics = JArray.Parse(responseContent).ToObject<Statistic[]>();
			return statistics;
		}

		/// <summary>
		/// Get email statistics for the given categories. If you don’t pass any parameters, the endpoint will return a sum for each category 10 at a time.
		/// See: https://sendgrid.com/docs/API_Reference/Web_API_v3/Stats/categories.html
		/// </summary>
		/// <param name="categories">The categories to get statistics for, up to 10</param>
		/// <param name="startDate">The starting date of the statistics to retrieve.</param>
		/// <param name="endDate">The end date of the statistics to retrieve. Defaults to today.</param>
		/// <param name="aggregatedBy">How to group the statistics, must be day|week|month</param>
		/// <returns></returns>
		public async Task<Statistic[]> GetCategoriesStatisticsAsync(IEnumerable<string> categories, DateTime startDate, DateTime? endDate = null, AggregateBy aggregatedBy = AggregateBy.None, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["start_date"] = startDate.ToString("yyyy-MM-dd");
			if (endDate.HasValue) query["end_date"] = endDate.Value.ToString("yyyy-MM-dd");
			if (aggregatedBy != AggregateBy.None) query["aggregated_by"] = aggregatedBy.GetDescription();
			if (categories != null && categories.Any())
			{
				foreach (var category in categories)
				{
					query.Add("categories", category);
				}
			}

			var response = await _client.GetAsync(string.Format("/categories/stats?{0}", query), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var statistics = JArray.Parse(responseContent).ToObject<Statistic[]>();
			return statistics;
		}

		/// <summary>
		/// Get email statistics for the given subusers. You can add up to 10 subusers parameters, one for each subuser you want stats for.
		/// See: https://sendgrid.com/docs/API_Reference/Web_API_v3/Stats/subusers.html
		/// </summary>
		/// <param name="subusers">The subusers to get statistics for, up to 10</param>
		/// <param name="startDate">The starting date of the statistics to retrieve.</param>
		/// <param name="endDate">The end date of the statistics to retrieve. Defaults to today.</param>
		/// <param name="aggregatedBy">How to group the statistics, must be day|week|month</param>
		/// <returns></returns>
		public async Task<Statistic[]> GetSubusersStatisticsAsync(IEnumerable<string> subusers, DateTime startDate, DateTime? endDate = null, AggregateBy aggregatedBy = AggregateBy.None, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["start_date"] = startDate.ToString("yyyy-MM-dd");
			if (endDate.HasValue) query["end_date"] = endDate.Value.ToString("yyyy-MM-dd");
			if (aggregatedBy != AggregateBy.None) query["aggregated_by"] = aggregatedBy.GetDescription();
			if (subusers != null && subusers.Any())
			{
				foreach (var subuser in subusers)
				{
					query.Add("subusers", subuser);
				}
			}

			var response = await _client.GetAsync(string.Format("/subusers/stats?{0}", query), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var statistics = JArray.Parse(responseContent).ToObject<Statistic[]>();
			return statistics;
		}

		/// <summary>
		/// Gets email statistics by country and state/province. Only supported for US and CA.
		/// See: https://sendgrid.com/docs/API_Reference/Web_API_v3/Stats/advanced.html
		/// </summary>
		/// <param name="country">US|CA</param>
		/// <param name="startDate">The starting date of the statistics to retrieve.</param>
		/// <param name="endDate">The end date of the statistics to retrieve. Defaults to today.</param>
		/// <param name="aggregatedBy">How to group the statistics, must be day|week|month</param>
		/// <returns></returns>
		public async Task<Statistic[]> GetCountryStatisticsAsync(string country, DateTime startDate, DateTime? endDate = null, AggregateBy aggregatedBy = AggregateBy.None, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["start_date"] = startDate.ToString("yyyy-MM-dd");
			if (endDate.HasValue) query["end_date"] = endDate.Value.ToString("yyyy-MM-dd");
			if (aggregatedBy != AggregateBy.None) query["aggregated_by"] = aggregatedBy.GetDescription();
			if (!string.IsNullOrEmpty(country)) query["country"] = country;

			var response = await _client.GetAsync(string.Format("/geo/stats?{0}", query), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var statistics = JArray.Parse(responseContent).ToObject<Statistic[]>();
			return statistics;
		}

		/// <summary>
		/// Gets email statistics by device type
		/// See: https://sendgrid.com/docs/API_Reference/Web_API_v3/Stats/advanced.html
		/// </summary>
		/// <param name="startDate">The starting date of the statistics to retrieve.</param>
		/// <param name="endDate">The end date of the statistics to retrieve. Defaults to today.</param>
		/// <param name="aggregatedBy">How to group the statistics, must be day|week|month</param>
		/// <returns></returns>
		public async Task<Statistic[]> GetDeviceTypesStatisticsAsync(DateTime startDate, DateTime? endDate = null, AggregateBy aggregatedBy = AggregateBy.None, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["start_date"] = startDate.ToString("yyyy-MM-dd");
			if (endDate.HasValue) query["end_date"] = endDate.Value.ToString("yyyy-MM-dd");
			if (aggregatedBy != AggregateBy.None) query["aggregated_by"] = aggregatedBy.GetDescription();

			var response = await _client.GetAsync(string.Format("/devices/stats?{0}", query), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var statistics = JArray.Parse(responseContent).ToObject<Statistic[]>();
			return statistics;
		}

		/// <summary>
		/// Get email statistics by client type
		/// See: https://sendgrid.com/docs/API_Reference/Web_API_v3/Stats/advanced.html
		/// </summary>
		/// <param name="startDate">The starting date of the statistics to retrieve.</param>
		/// <param name="endDate">The end date of the statistics to retrieve. Defaults to today.</param>
		/// <param name="aggregatedBy">How to group the statistics, must be day|week|month</param>
		/// <returns></returns>
		public async Task<Statistic[]> GetClientTypesStatisticsAsync(DateTime startDate, DateTime? endDate = null, AggregateBy aggregatedBy = AggregateBy.None, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["start_date"] = startDate.ToString("yyyy-MM-dd");
			if (endDate.HasValue) query["end_date"] = endDate.Value.ToString("yyyy-MM-dd");
			if (aggregatedBy != AggregateBy.None) query["aggregated_by"] = aggregatedBy.GetDescription();

			var response = await _client.GetAsync(string.Format("/clients/stats?{0}", query), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var statistics = JArray.Parse(responseContent).ToObject<Statistic[]>();
			return statistics;
		}

		/// <summary>
		/// Gets email statistics by mailbox provider
		/// See: https://sendgrid.com/docs/API_Reference/Web_API_v3/Stats/advanced.html
		/// </summary>
		/// <param name="providers">The mailbox providers to get statistics for, up to 10</param>
		/// <param name="startDate">The starting date of the statistics to retrieve.</param>
		/// <param name="endDate">The end date of the statistics to retrieve. Defaults to today.</param>
		/// <param name="aggregatedBy">How to group the statistics, must be day|week|month</param>
		/// <returns></returns>
		public async Task<Statistic[]> GetInboxProvidersStatisticsAsync(IEnumerable<string> providers, DateTime startDate, DateTime? endDate = null, AggregateBy aggregatedBy = AggregateBy.None, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["start_date"] = startDate.ToString("yyyy-MM-dd");
			if (endDate.HasValue) query["end_date"] = endDate.Value.ToString("yyyy-MM-dd");
			if (aggregatedBy != AggregateBy.None) query["aggregated_by"] = aggregatedBy.GetDescription();
			if (providers != null && providers.Any())
			{
				foreach (var provider in providers)
				{
					query.Add("mailbox_providers", provider);
				}
			}

			var response = await _client.GetAsync(string.Format("/mailbox_providers/stats?{0}", query), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var statistics = JArray.Parse(responseContent).ToObject<Statistic[]>();
			return statistics;
		}

		/// <summary>
		/// Gets email statistics by browser
		/// See: https://sendgrid.com/docs/API_Reference/Web_API_v3/Stats/advanced.html
		/// </summary>
		/// <param name="browsers">The browsers to get statistics for, up to 10</param>
		/// <param name="startDate">The starting date of the statistics to retrieve.</param>
		/// <param name="endDate">The end date of the statistics to retrieve. Defaults to today.</param>
		/// <param name="aggregatedBy">How to group the statistics, must be day|week|month</param>
		/// <returns></returns>
		public async Task<Statistic[]> GetBrowsersStatisticsAsync(IEnumerable<string> browsers, DateTime startDate, DateTime? endDate = null, AggregateBy aggregatedBy = AggregateBy.None, CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["start_date"] = startDate.ToString("yyyy-MM-dd");
			if (endDate.HasValue) query["end_date"] = endDate.Value.ToString("yyyy-MM-dd");
			if (aggregatedBy != AggregateBy.None) query["aggregated_by"] = aggregatedBy.GetDescription();
			if (browsers != null && browsers.Any())
			{
				foreach (var browser in browsers)
				{
					query.Add("browsers", browser);
				}
			}

			var response = await _client.GetAsync(string.Format("/browsers/stats?{0}", query), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var statistics = JArray.Parse(responseContent).ToObject<Statistic[]>();
			return statistics;
		}
	}
}