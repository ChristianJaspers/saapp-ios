using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	class HttpClientFileUploader
	{
		public const string HttpMethodPost = "POST";
		public const string HttpMethodPut = "PUT";

		public const string HttpHeaderTitleContentType = "Content-Type";

		private FileUploadResult UnknownErrorResult;

		private string authorizationToken;

		public HttpClientFileUploader(string authorizationToken)
		{
			this.authorizationToken = authorizationToken;

			UnknownErrorResult = new FileUploadResult
			{
				// TODO - replace with actual unknown error code and localized message
				Error = new ServiceAccessError(-1000, "Something went wrong. Please try again later.")
			};
		}

		public async Task<FileUploadResult> UploadFileAsync(string uploadUrl, string localFilePath, string parameterName, string mimeType, string method = HttpMethodPost)
		{
			var isPostOrPut = method.Equals(HttpMethodPost) || method.Equals(HttpMethodPut);
			Debug.Assert(isPostOrPut, "UploadFileAsync expected HTTP method to be POST or PUT, but found " + method);

			if (!File.Exists(localFilePath))
			{
				// TODO - internal error handling
			}

			// TODO - check network availability

			try
			{
				return await PerformUploadAsync(uploadUrl, localFilePath, parameterName, mimeType, method);
			}
			catch (Exception e)
			{
				Debug.WriteLine("An exception occured during upload: " + e);
				return UnknownErrorResult;
			}
		}

		private async Task<FileUploadResult> PerformUploadAsync(string uploadUrl, string localFilePath, string parameterName, string mimeType, string method)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + authorizationToken + "\"");

				var boundary = "Upload----" + DateTime.Now.Ticks;
				using (var content = new MultipartFormDataContent(boundary))
				{
					using (var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
					{
						var streamContent = new StreamContent(fileStream);
						streamContent.Headers.Remove(HttpHeaderTitleContentType);
						streamContent.Headers.Add(HttpHeaderTitleContentType, mimeType);

						var fileName = Path.GetFileName(localFilePath);
						Debug.WriteLine("File name: " + fileName);
						content.Add(streamContent, parameterName, fileName);

						if (method.Equals(HttpMethodPost))
						{
							using (var response = await client.PostAsync(uploadUrl, content))
							{
								return await HandleResponseAsync(response);
							}
						}
						else if (method.Equals(HttpMethodPut))
						{
							using (var response = await client.PutAsync(uploadUrl, content))
							{
								return await HandleResponseAsync(response);
							}
						}
						else
						{
							return UnknownErrorResult;
						}
					}
				}
			}
		}

		private async Task<FileUploadResult> HandleResponseAsync(HttpResponseMessage response)
		{
			var responseText = await response.Content.ReadAsStringAsync();
			var result = new FileUploadResult();

			Debug.WriteLine("Response code: " + response.StatusCode + " message: " + responseText);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				var uploadSuccessResponse = JsonConvert.DeserializeObject<FileUploadSuccessResponse>(responseText);
				var isSuccess = uploadSuccessResponse.User != null;
				if (isSuccess)
				{
					result.User = uploadSuccessResponse.User;
				}
				else
				{
					result = UnknownErrorResult;
				}
			}
			else
			{
				var uploadFailureResponse= JsonConvert.DeserializeObject<FileUploadFailureResponse>(responseText);
				var isKnownError = uploadFailureResponse.Error != null;
				if (isKnownError)
				{
					result.Error = uploadFailureResponse.Error;
				}
				else
				{
					result = UnknownErrorResult;
				}
			}

			return result;
		}
	}

	public class FileUploadSuccessResponse
	{
		[JsonPropertyAttribute(PropertyName = "user")]
		public User User { get; set; }
	}

	public class FileUploadFailureResponse
	{
		[JsonPropertyAttribute(PropertyName = "error")]
		public ServiceAccessError Error { get; set; }
	}

	public class FileUploadResult
	{
		public bool IsSuccess { get { return Error == null; } }
		public User User { get; set; }
		public ServiceAccessError Error { get; set; }
	}
}

