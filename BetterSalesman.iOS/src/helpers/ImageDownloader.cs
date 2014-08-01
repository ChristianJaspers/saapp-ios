using System;
using System.Threading.Tasks;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using BetterSalesman.Core.ServiceAccessLayer;
using System.Diagnostics;

namespace BetterSalesman.iOS
{
	public class ImageDownloader
	{
		public ImageDownloader()
		{
		}

		// TODO - add reachability check
		public static async Task<ImageDownloadResult> DownloadImage(string url)
		{
			ImageDownloadResult result = null;

			await Task.Run(() =>
			{
				try
				{
					using (var downloadUrl = new NSUrl(url))
					{
						using (var imageData = NSData.FromUrl(downloadUrl))
						{
							var image = UIImage.LoadFromData(imageData);
							result = new ImageDownloadResult(image);
						}
					}		
				}
				catch (Exception e)
				{
					Debug.WriteLine("Exception when downloding file: " + url + " - exception: " + e);
					result = new ImageDownloadResult(ServiceAccessError.ErrorUnknown);
				}
			});

			return result;
		}
	}

	public class ImageDownloadResult
	{
		public ImageDownloadResult() : base()
		{
		}

		public ImageDownloadResult(UIImage image) : base()
		{
			Image = image;
		}

		public ImageDownloadResult(ServiceAccessError error) : base()
		{
			Error = error;
		}

		public bool IsSuccess { get { return Error == null; } }
		public UIImage Image { get; set; }
		public ServiceAccessError Error { get; set; }
	}
}

