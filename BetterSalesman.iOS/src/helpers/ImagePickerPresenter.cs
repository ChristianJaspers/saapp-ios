using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.MobileCoreServices;
using System.Collections.Generic;
using System.Diagnostics;

namespace BetterSalesman.iOS
{
	/// <summary>
	/// Finished picking event handler.
	/// </summary>
	public delegate void FinishedPickingEventHandler(bool didPickAnImage, UIImage pickedImage);

	/// <summary>
	/// This class is responsible for configuring and displaying UIActionSheet followed by UIImagePickerController.
	/// </summary>
	public class ImagePickerPresenter
	{
		public const string ImageMediaType = "public.image";

		public const UIImagePickerControllerSourceType CameraPickerType = UIImagePickerControllerSourceType.Camera;
		public const UIImagePickerControllerSourceType PhotoLibraryPickerType = UIImagePickerControllerSourceType.PhotoLibrary;

		public event FinishedPickingEventHandler FinishedPicking;

		private UIImagePickerController imagePicker;
		private UIActionSheet mediaSourceTypePicker;

		// TODO - add to localized strings
		private string MediaSourceTypePickerLabel = "Choose source".t();
		private string CameraButtonLabel = "Camera".t();
		private string PhotoLibraryButtonLabel = "Photo library".t();
		private string CancelButtonLabel = "Cancel".t();

		private Dictionary<int, UIImagePickerControllerSourceType> buttonIndexesForMediaSourceTypes;
		private int lastAddedMediaSourceTypeButtonIndex;

		public ImagePickerPresenter()
		{
			buttonIndexesForMediaSourceTypes = new Dictionary<int, UIImagePickerControllerSourceType>();
		}

		public void ShowImagePickerTypeSelection(UIViewController viewController)
		{
			buttonIndexesForMediaSourceTypes.Clear();
			lastAddedMediaSourceTypeButtonIndex = -1;

			mediaSourceTypePicker = new UIActionSheet(MediaSourceTypePickerLabel);

			AddButtonToMediaSourceTypePickerIfTypeAvailable(CameraButtonLabel, CameraPickerType);
			AddButtonToMediaSourceTypePickerIfTypeAvailable(PhotoLibraryButtonLabel, PhotoLibraryPickerType);
			mediaSourceTypePicker.AddButton(CancelButtonLabel);

			mediaSourceTypePicker.Clicked += (object sender, UIButtonEventArgs e) =>
			{
				if (e.ButtonIndex <= lastAddedMediaSourceTypeButtonIndex)
				{
					var selectedMediaSourceType = buttonIndexesForMediaSourceTypes[e.ButtonIndex];
					ShowImagePicker(viewController, selectedMediaSourceType);
				}
			};

			mediaSourceTypePicker.ShowInView(viewController.View);
		}

		private void ShowImagePicker(UIViewController viewController, UIImagePickerControllerSourceType pickerType)
		{
			imagePicker = new UIImagePickerController();

			imagePicker.SourceType = pickerType;
			// only allow photos (not videos)
			imagePicker.MediaTypes = new string[] { UTType.Image };
			if (pickerType == UIImagePickerControllerSourceType.Camera)
			{
				imagePicker.ShowsCameraControls = true;
			}

			imagePicker.FinishedPickingMedia += (s, e) =>
			{
				viewController.InvokeOnMainThread(() =>
				{
					bool didPickAnImage = e.MediaType == ImageMediaType && e.OriginalImage != null;
					OnFinishedPicking(didPickAnImage, e.OriginalImage);
				});
			};

			imagePicker.Canceled += (object s, EventArgs e) => 
			{
				OnFinishedPicking(false, null);
			};

			imagePicker.ModalInPopover = true;
			imagePicker.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
			imagePicker.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;

			viewController.PresentViewController(imagePicker, true, null);
		}

		private void OnFinishedPicking(bool didPickAnImage, UIImage pickedImage)
		{
			imagePicker.DismissViewController(true, null);
			if (FinishedPicking != null)
			{
				FinishedPicking(didPickAnImage, pickedImage);
			}
		}

		private bool IsPickerTypeAvailable(UIImagePickerControllerSourceType type)
		{
			return UIImagePickerController.IsSourceTypeAvailable(type);
		}

		private void AddButtonToMediaSourceTypePickerIfTypeAvailable(string buttonLabel, UIImagePickerControllerSourceType mediaSourceType)
		{
			if (IsPickerTypeAvailable(mediaSourceType))
			{
				mediaSourceTypePicker.AddButton(buttonLabel);
				lastAddedMediaSourceTypeButtonIndex += 1;
				buttonIndexesForMediaSourceTypes.Add(lastAddedMediaSourceTypeButtonIndex, mediaSourceType);
			}
		}
	}
}

