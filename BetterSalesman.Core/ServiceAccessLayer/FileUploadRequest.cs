using System;
using System.Net;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	public delegate void FileUploadCompletedEventHandler(string remoteFileUrl);
	public delegate void FileUploadProgressUpdatedEventHandler(int progressPercentage);

	public class FileUploadRequest
	{
		// TODO - extract and localize
		public const int UnknownNetworkErrorCode = -1001;
		public const string UnknownNetworErrorMessage = "An unknown error occured. Please try again later.";
		public const int FileNotFoundErrorCode = -1000;
		public const string FileNotFoundErrorMessage = "Couldn't find file you selected. Please select another file.";

		public event FileUploadCompletedEventHandler Success;
		public event HttpRequestFailureEventHandler Failure;
		public event FileUploadProgressUpdatedEventHandler ProgressUpdated;

		private WebClient client;

		public FileUploadRequest()
		{
		}

		~FileUploadRequest()
		{
			if (client != null) 
			{
				client.Dispose();
			}
		}

		public void Perform(string localFilePath)
		{
			if (!File.Exists(localFilePath)) 
			{
				OnFailure(FileNotFoundErrorCode, FileNotFoundErrorMessage);
				return;
			}

			using (client = new WebClient()) 
			{
				client.UploadProgressChanged += OnProgressUpdated;

				client.UploadFileCompleted += OnUploadCompleted;

				Debug.WriteLine("Begining upload");

				// TODO - change to production URI
				client.UploadFileAsync(new Uri("http://roweo.pl/api/user/image_upload", UriKind.Absolute), localFilePath);
			}
		}

		public void Cancel()
		{
			if (client != null) 
			{
				client.CancelAsync();
			}
		}

		private void OnUploadCompleted(object sender, UploadFileCompletedEventArgs e)
		{
			Debug.WriteLine("Upload completed");

			var result = e.Result;
			var error = e.Error;

			if (result != null) 
			{
				var resultString = System.Text.Encoding.UTF8.GetString(result);

				OnSuccess(resultString);
				return;
			} 
			else 
			{
				// TODO - use debugger to find out where the JSON is contained, then add parsing code here
				if (error != null) 
				{
					Debug.WriteLine("Upload error: " + error.Message + "\nStackTrace: " + error.StackTrace);
					OnFailure(UnknownNetworkErrorCode, UnknownNetworErrorMessage);
				} 
				else 
				{
					OnFailure(UnknownNetworkErrorCode, UnknownNetworErrorMessage);
				}
			}
		}

		private void OnProgressUpdated(object sender, UploadProgressChangedEventArgs e)
		{
			if (ProgressUpdated != null) 
			{
				ProgressUpdated(e.ProgressPercentage);
			}
		}

		private void OnSuccess(string response)
		{
			if (Success != null) 
			{
				var deserializedResponse = ParseAndDeserializeResponse(response);

				Success(deserializedResponse.FileRemoteUrl);
			}
		}

		private void OnFailure(int errorCode, string errorMessage)
		{
			if (Failure != null) 
			{
				Failure(errorCode);
			}
		}
			
		private FileUploadResponse ParseAndDeserializeResponse(string response)
		{
			var errors = new List<string>();
			JsonSerializerSettings jsonSerializationSettings = new JsonSerializerSettings
			{
				Error = (sender, args) =>
				{
					errors.Add(args.ErrorContext.Error.Message);
					args.ErrorContext.Handled = true;
				}
			};

			var deserailizedResponse = JsonConvert.DeserializeObject<FileUploadResponse>(response, jsonSerializationSettings);

			if (errors.Any())
			{
				Debug.WriteLine("ERROR! Encountered error when deserializing response: " + errors[0]);
			}

			return deserailizedResponse;
		}

		internal class FileUploadResponse
		{
			// TODO - change to production value
			[JsonProperty("image_remote_path")]
			public string FileRemoteUrl { get; set; }
		}
	}
}

