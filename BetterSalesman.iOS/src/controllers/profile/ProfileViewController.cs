using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Collections.Generic;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.BusinessLayer.Managers;
using System.IO;
using System.Threading.Tasks;

namespace BetterSalesman.iOS
{
	partial class ProfileViewController : BaseUIViewController
	{
		private const string ImageMediaType = "public.image";

		private UIImagePickerController imagePicker;

		private User user;

		public ProfileViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			BackButton.TouchUpInside += (sender, e) => DismissViewController(true, null);

			ProfileImageEditButton.TouchUpInside += (sender, @event) => 
			{
				ShowImagePickerTypeSelection();
			};

//			LoadUser();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

//			ShowIndicator();
//
//			ServiceProviderUser.Instance.Profile(async result =>
//				{
//					await UserSessionManager.Instance.FetchUser(obj =>
//					{
//						LoadUser();
//						// TODO refresh UI here?
//					});
//
//					HideIndicator();
//				},
//				errorCode =>
//				{
//					HideIndicator();  
//					ShowAlert(I18n.ErrorConnectionTimeout);
//				});
		}

		private void ShowImagePickerTypeSelection()
		{
			var CameraPickerType = UIImagePickerControllerSourceType.Camera;
			var PhotoLibraryPickerType = UIImagePickerControllerSourceType.PhotoLibrary;
			var SavedPhotosPickerType = UIImagePickerControllerSourceType.Camera;

			var buttonIndexesToSourceTypes = new Dictionary<int, UIImagePickerControllerSourceType>();
			var lastButtonIndex = -1;

			// TODO - localize all UI strings
			UIAlertView alert = new UIAlertView() 
			{
				Title = "Pick image".t(),
			};

			if (IsPickerTypeAvailable(CameraPickerType))
			{
				alert.AddButton ("Camera".t());
				lastButtonIndex += 1;
				buttonIndexesToSourceTypes.Add(lastButtonIndex, CameraPickerType); 
			}

			if (IsPickerTypeAvailable(PhotoLibraryPickerType))
			{
				alert.AddButton ("Photo library".t());
				lastButtonIndex += 1;
				buttonIndexesToSourceTypes.Add(lastButtonIndex, PhotoLibraryPickerType);
			}

			if (IsPickerTypeAvailable(SavedPhotosPickerType))
			{
				alert.AddButton ("Photo library".t());
				lastButtonIndex += 1;
				buttonIndexesToSourceTypes.Add(lastButtonIndex, SavedPhotosPickerType);
			}

			alert.AddButton("Cancel".t());

			// last button added is the 'cancel' button (index of '2')
			alert.Clicked += (object a, UIButtonEventArgs b) =>
			{
				ShowImagePicker(buttonIndexesToSourceTypes[b.ButtonIndex]);
			};

			alert.Show ();
		}

		private bool IsPickerTypeAvailable(UIImagePickerControllerSourceType type)
		{
			return UIImagePickerController.IsSourceTypeAvailable(type);
		}

		private void ShowImagePicker(UIImagePickerControllerSourceType pickerType)
		{
			imagePicker = new UIImagePickerController();

			imagePicker.SourceType = pickerType;
			if (pickerType == UIImagePickerControllerSourceType.Camera)
			{
				imagePicker.ShowsCameraControls = true;
			}
			imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(pickerType);

			imagePicker.FinishedPickingMedia += async (s, e) =>
			{
				await HandleFilePickedAndLoaded(s, e);
			};

			imagePicker.Canceled += (object s, EventArgs e) => 
			{
				imagePicker.DismissViewController(true, null);
			};

			imagePicker.ModalInPopover = true;
			imagePicker.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
			imagePicker.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;

			PresentViewController(imagePicker, true, null);
		}

		private async Task HandleFilePickedAndLoaded(object s, UIImagePickerMediaPickedEventArgs e)
		{
			imagePicker.DismissViewController (true, null);

			Debug.WriteLine("MediaUrl: " + e.MediaUrl);
			if (e.MediaType == ImageMediaType)
			{
				await UploadImage(e.OriginalImage);
			}
		}

		private async Task UploadImage(UIImage image)
		{
			var imageFilePath = await SaveImageToTemporaryFilePng(image);

			var fileUploadRequest = new FileUploadRequest ();

			fileUploadRequest.ProgressUpdated += (int progressPercentage) => Debug.WriteLine ("Upload progress: " + progressPercentage);

			fileUploadRequest.Success += async (string remoteFileUrl) => 
			{
				Debug.WriteLine ("Upload complete - file URL: " + remoteFileUrl);
				RemoveTemporaryFile(imageFilePath);

				UIImage remoteImage = await Task<UIImage>.Run(() =>
				{
					// TODO - check if URL is valid and if connection is available
					var data = NSData.FromUrl(new NSUrl(remoteFileUrl));
					return UIImage.LoadFromData(data);
				});

				InvokeOnMainThread(() =>
				{
					ProfileImageView.Image = remoteImage;
				});
			};

			fileUploadRequest.Failure += (int errorCode) =>
			{
				Debug.WriteLine ("Upload failed: " + errorCode);
				RemoveTemporaryFile(imageFilePath);
			};

			fileUploadRequest.Perform(imageFilePath);
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
					Console.WriteLine("NOT saved file because" + error.LocalizedDescription);
				}
			});

			return temporaryImageFilePath;
		}

		// TODO - might require some extra checks to see if file can we opened in ReadWrite mode
		public void RemoveTemporaryFile(string temporaryFilePath)
		{
			if (File.Exists(temporaryFilePath))
			{
				File.Delete(temporaryFilePath);
			}
		}
			
		void LoadUser()
		{
			user = UserManager.LoggedInUser();
		}
	}
}
