using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Collections.Generic;

namespace BetterSalesman.iOS
{
	partial class ProfileViewController : UIViewController
	{
		private const string ImageMediaType = "public.image";

		private UIImagePickerController imagePicker;

		public ProfileViewController (IntPtr handle) : base (handle)
		{
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

			imagePicker.FinishedPickingMedia += (object s, UIImagePickerMediaPickedEventArgs e) => 
			{
				Debug.WriteLine("MediaUrl: " + e.MediaUrl);
				if (e.MediaType == ImageMediaType)
				{
					ProfileImageView.Image = e.OriginalImage;

					// TODO - upload
				}

				imagePicker.DismissViewController(true, null);
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
	}
}
