using System;
using System.Threading.Tasks;
using System.IO;
using MonoTouch.UIKit;
using MonoTouch.Foundation;


namespace BetterSalesman.iOS
{
	public class ImageFilesManagementHelper
	{
		static ImageFilesManagementHelper sharedInstance;
		static object locker = new Object();

		public static ImageFilesManagementHelper SharedInstance
		{
			get
			{
				if (sharedInstance == null)
				{
					lock (locker)
					{
						if (sharedInstance == null)
						{
							sharedInstance = new ImageFilesManagementHelper();
						}
					}
				}

				return sharedInstance;
			}
		}

		public ImageFilesManagementHelper()
		{
		}

		/// <summary>
		/// Saves the image to temporary file in png format.
		/// </summary>
		/// <returns>Path to temporary file representing image provide as paramater</returns>
		/// <param name="image">The image to be saved to a tempoarary png file</param>
		public async Task<string> SaveImageToTemporaryFilePng(UIImage image)
		{
			var uniqueFileNamePortion = Guid.NewGuid().ToString();
			var temporaryImageFileName = string.Format("{0}.png", uniqueFileNamePortion);

			var documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var temporaryStorageFolderPath = Path.Combine(documentsFolderPath, "..", "tmp");

			var temporaryImageFilePath = Path.Combine(temporaryStorageFolderPath, temporaryImageFileName);

			var imageData = image.AsPNG();
			await Task.Run(() => 
			{
				//File.WriteAllBytes(temporaryImageFilePath, imageData);
				NSError error = null;
				if (imageData.Save(temporaryImageFilePath, false, out error)) 
				{
					Console.WriteLine("saved file");
				} else 
				{
					Console.WriteLine("ERROR! Did NOT SAVE file because" + error.LocalizedDescription);
				}
			});

			return temporaryImageFilePath;
		}

		// TODO - might require some extra checks to see if file can we opened in ReadWrite mode
		/// <summary>
		/// Removes the temporary file at given path. 
		/// 
		/// NOTE! Use this method only to remove the files that were created using SaveImageToTemporaryFilePng with the path returned by that method.
		/// </summary>
		/// <param name="temporaryFilePath">Temporary file path.</param>
		public void RemoveTemporaryFile(string temporaryFilePath)
		{
			if (File.Exists(temporaryFilePath))
			{
				File.Delete(temporaryFilePath);
			}
		}
	}
}

