﻿using Newtonsoft.Json.Linq;
using StrongGrid.Model;
using StrongGrid.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StrongGrid.Resources
{
	public class Bounces
	{
		private readonly string _endpoint;
		private readonly IClient _client;

		/// <summary>
		/// Constructs the SendGrid Bounces object.
		/// See https://sendgrid.com/docs/API_Reference/Web_API_v3/bounces.html
		/// </summary>
		/// <param name="client">SendGrid Web API v3 client</param>
		/// <param name="endpoint">Resource endpoint, do not prepend slash</param>
		public Bounces(IClient client, string endpoint = "/suppression/bounces")
		{
			_endpoint = endpoint;
			_client = client;
		}

		/// <summary>
		/// Get a list of bounces
		/// </summary>
		/// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/bounces.html</returns>
		public async Task<Bounce[]> GetAsync(DateTime? start = null, DateTime? end = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var endpoint = string.Format("{0}?start_time={1}&end_time={2}", _endpoint, start.Value.ToUnixTime().ToString(), end.Value.ToUnixTime().ToString());
			var response = await _client.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var bounces = JArray.Parse(responseContent).ToObject<Bounce[]>();
			return bounces;
		}

		/// <summary>
		/// Get a list of bounces for a given email address
		/// </summary>
		/// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/bounces.html</returns>
		public async Task<Bounce[]> GetAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.GetAsync(string.Format("{0}/{1}", _endpoint, email), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var bounces = JArray.Parse(responseContent).ToObject<Bounce[]>();
			return bounces;
		}

		/// <summary>
		/// Delete all bounces
		/// </summary>
		/// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/bounces.html</returns>
		public async Task DeleteAllAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject(new JProperty("delete_all", true));
			var response = await _client.DeleteAsync(_endpoint, data, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();
		}

		/// <summary>
		/// Delete bounces for a specified group of email addresses
		/// </summary>
		/// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/bounces.html</returns>
		public async Task DeleteAsync(IEnumerable<string> emails, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject(new JProperty("emails", JArray.FromObject(emails.ToArray())));
			var response = await _client.DeleteAsync(_endpoint, data, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();
		}

		/// <summary>
		/// Delete bounces for a specified email address
		/// </summary>
		/// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/bounces.html</returns>
		public async Task DeleteAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.DeleteAsync(string.Format("{0}/{1}", _endpoint, email), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();
		}
	}
}
