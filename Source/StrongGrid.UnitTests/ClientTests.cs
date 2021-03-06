﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace StrongGrid.UnitTests
{
	[TestClass]
	public class ClientTests
	{
		private const string API_KEY = "my_api_key";
		private const string MEDIA_TYPE = "application/json";

		[TestMethod]
		public void Version_is_not_empty()
		{
			// Arrange
			var client = new Client(API_KEY);

			// Act
			var result = client.Version;

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(result));
		}

		[TestMethod]
		public void GetAsync_success()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Get, "https://api.sendgrid.com/v3/myendpoint")
				.With(request => request.Content == null)
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.GetAsync("myendpoint", CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void GetAsync_exception()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Get, "https://api.sendgrid.com/v3/myendpoint")
				.With(request => request.Content == null)
				.Throw(new System.Exception("Let's pretend something wrong happened"));

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.GetAsync("myendpoint", CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsFalse(result.IsSuccessStatusCode);
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
		}

		[TestMethod]
		public void PostAsync_without_jObject()
		{
			// Arrange
			var data = (JObject)null;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Post, "https://api.sendgrid.com/v3/myendpoint")
				.With(request => request.Content == null)
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.PostAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void PostAsync_with_jObject()
		{
			// Arrange
			var data = new JObject();
			data.Add("property1", "Abc");
			data.Add("property2", 123);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Post, "https://api.sendgrid.com/v3/myendpoint")
				.WithContent(data.ToString())
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.PostAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void PostAsync_without_jArray()
		{
			// Arrange
			var data = (JArray)null;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Post, "https://api.sendgrid.com/v3/myendpoint")
				.With(request => request.Content == null)
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.PostAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void PostAsync_with_jArray()
		{
			// Arrange
			var object1 = new JObject();
			object1.Add("property1", "Abc");
			object1.Add("property2", 123);

			var object2 = new JObject();
			object2.Add("property1", "Xyz");
			object2.Add("property2", 987);

			var data = JArray.FromObject(new[] { object1, object2 });

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Post, "https://api.sendgrid.com/v3/myendpoint")
				.WithContent(data.ToString())
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.PostAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void DeleteAsync_without_content()
		{
			// Arrange
			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Delete, "https://api.sendgrid.com/v3/myendpoint")
				.With(request => request.Content == null)
				.Respond(HttpStatusCode.NoContent);

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.DeleteAsync("myendpoint", CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void DeleteAsync_without_jobject()
		{
			// Arrange
			var data = (JObject)null;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Delete, "https://api.sendgrid.com/v3/myendpoint")
				.With(request => request.Content == null)
				.Respond(HttpStatusCode.NoContent);

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.DeleteAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void DeleteAsync_with_jobject()
		{
			// Arrange
			var data = new JObject();
			data.Add("property1", "Abc");
			data.Add("property2", 123);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Delete, "https://api.sendgrid.com/v3/myendpoint")
				.WithContent(data.ToString())
				.Respond(HttpStatusCode.NoContent);

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.DeleteAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void DeleteAsync_without_jArray()
		{
			// Arrange
			var data = (JArray)null;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Delete, "https://api.sendgrid.com/v3/myendpoint")
				.With(request => request.Content == null)
				.Respond(HttpStatusCode.NoContent);

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.DeleteAsync("myendpoint", data).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void DeleteAsync_with_jArray()
		{
			// Arrange
			var object1 = new JObject();
			object1.Add("property1", "Abc");
			object1.Add("property2", 123);

			var object2 = new JObject();
			object2.Add("property1", "Xyz");
			object2.Add("property2", 987);

			var data = JArray.FromObject(new[] { object1, object2 });

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Delete, "https://api.sendgrid.com/v3/myendpoint")
				.WithContent(data.ToString())
				.Respond(HttpStatusCode.NoContent);

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.DeleteAsync("myendpoint", data).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void PatchAsync_without_jObject()
		{
			// Arrange
			var data = (JObject)null;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(new HttpMethod("PATCH"), "https://api.sendgrid.com/v3/myendpoint")
				.With(request => request.Content == null)
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.PatchAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void PatchAsync_with_jObject()
		{
			// Arrange
			var data = new JObject();
			data.Add("property1", "Abc");
			data.Add("property2", 123);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(new HttpMethod("PATCH"), "https://api.sendgrid.com/v3/myendpoint")
				.WithContent(data.ToString())
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.PatchAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void PatchAsync_without_jArray()
		{
			// Arrange
			var data = (JArray)null;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(new HttpMethod("PATCH"), "https://api.sendgrid.com/v3/myendpoint")
				.With(request => request.Content == null)
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.PatchAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void PatchAsync_with_jArray()
		{
			// Arrange
			var object1 = new JObject();
			object1.Add("property1", "Abc");
			object1.Add("property2", 123);

			var object2 = new JObject();
			object2.Add("property1", "Xyz");
			object2.Add("property2", 987);

			var data = JArray.FromObject(new[] { object1, object2 });

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(new HttpMethod("PATCH"), "https://api.sendgrid.com/v3/myendpoint")
				.WithContent(data.ToString())
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.PatchAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void PutAsync_without_jObject()
		{
			// Arrange
			var data = (JObject)null;

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Put, "https://api.sendgrid.com/v3/myendpoint")
				.With(request => request.Content == null)
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.PutAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void PutAsync_with_jObject()
		{
			// Arrange
			var data = new JObject();
			data.Add("property1", "Abc");
			data.Add("property2", 123);

			var mockHttp = new MockHttpMessageHandler();
			mockHttp.When(HttpMethod.Put, "https://api.sendgrid.com/v3/myendpoint")
				.WithContent(data.ToString())
				.Respond("application/json", "{'name' : 'This is a test'}");

			var httpClient = new HttpClient(mockHttp);
			var client = new Client(apiKey: API_KEY, httpClient: httpClient);

			// Act
			var result = client.PutAsync("myendpoint", data, CancellationToken.None).Result;

			// Assert
			mockHttp.VerifyNoOutstandingExpectation();
			mockHttp.VerifyNoOutstandingRequest();
			Assert.IsTrue(result.IsSuccessStatusCode);
		}

		[TestMethod]
		public void Dispose()
		{
			// Arrange
			var client = new Client(apiKey: API_KEY);

			// Act
			client.Dispose();

			// Assert
			// Nothing to assert. We just want to confirm that 'Dispose' did not throw any exception
		}
	}
}
