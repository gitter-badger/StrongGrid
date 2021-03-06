﻿using Newtonsoft.Json.Linq;
using StrongGrid.Model;
using StrongGrid.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace StrongGrid.Resources
{
	public class UnsubscribeGroups
	{
		private readonly string _endpoint;
		private readonly IClient _client;

		/// <summary>
		/// Constructs the SendGrid UnsubscribeGroups object.
		/// See https://sendgrid.com/docs/API_Reference/Web_API_v3/Suppression_Management/groups.html
		/// </summary>
		/// <param name="client">SendGrid Web API v3 client</param>
		/// <param name="endpoint">Resource endpoint, do not prepend slash</param>
		public UnsubscribeGroups(IClient client, string endpoint = "/asm/groups")
		{
			_endpoint = endpoint;
			_client = client;
		}

		/// <summary>
		/// Retrieve all suppression groups associated with the user.
		/// </summary>
		/// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/Suppression_Management/groups.html</returns>
		public async Task<SuppressionGroup[]> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.GetAsync(_endpoint, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var groups = JArray.Parse(responseContent).ToObject<SuppressionGroup[]>();
			return groups;
		}

		/// <summary>
		/// Get information on a single suppression group.
		/// </summary>
		/// <param name="groupId">ID of the suppression group to delete</param>
		/// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/Suppression_Management/groups.html</returns>
		public async Task<SuppressionGroup> GetAsync(int groupId, CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.GetAsync(string.Format("{0}/{1}", _endpoint, groupId), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var group = JObject.Parse(responseContent).ToObject<SuppressionGroup>();
			return group;
		}

		/// <summary>
		/// Create a new suppression group.
		/// </summary>
		/// <param name="name">The name of the new suppression group</param>
		/// <param name="description">A description of the suppression group</param>
		/// <param name="isDefault">Default value is false</param>
		/// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/Suppression_Management/groups.html</returns>
		public async Task<SuppressionGroup> CreateAsync(string name, string description, bool isDefault, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject
			{
				{ "name", name },
				{ "description", description },
				{ "is_default", isDefault }
			};
			var response = await _client.PostAsync(_endpoint, data, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var group = JObject.Parse(responseContent).ToObject<SuppressionGroup>();
			return group;
		}

		/// <summary>
		/// Update an existing suppression group.
		/// </summary>
		/// <param name="name">The name of the new suppression group</param>
		/// <param name="description">A description of the suppression group</param>
		/// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/Suppression_Management/groups.html</returns>
		public async Task<SuppressionGroup> UpdateAsync(int groupId, string name = null, string description = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			var data = new JObject();
			if (name != null) data.Add("name", name);
			if (description != null) data.Add("description", description);

			var response = await _client.PatchAsync(string.Format("{0}/{1}", _endpoint, groupId), data, cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();

			var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var group = JObject.Parse(responseContent).ToObject<SuppressionGroup>();
			return group;
		}

		/// <summary>
		/// Delete a suppression group.
		/// </summary>
		/// <param name="groupId">ID of the suppression group to delete</param>
		/// <returns>https://sendgrid.com/docs/API_Reference/Web_API_v3/Suppression_Management/groups.html</returns>
		public async Task DeleteAsync(int groupId, CancellationToken cancellationToken = default(CancellationToken))
		{
			var response = await _client.DeleteAsync(string.Format("{0}/{1}", _endpoint, groupId), cancellationToken).ConfigureAwait(false);
			response.EnsureSuccess();
		}
	}
}
