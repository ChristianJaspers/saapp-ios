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
		private ImagePickerPresenter imagePickerPresenter;
		private User user;

		public ProfileViewController (IntPtr handle) : base (handle)
		{
			imagePickerPresenter = new ImagePickerPresenter ();
			imagePickerPresenter.FinishedPicking += async (bool didPickAnImage, UIImage pickedImage) => 
			{
				if (didPickAnImage)
				{
					await UploadImage(pickedImage);
				}
			};
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			BackButton.TouchUpInside += (sender, e) => DismissViewController(true, null);

			ProfileImageEditButton.TouchUpInside += (sender, @event) => 
			{
				imagePickerPresenter.ShowImagePickerTypeSelection(this);
			};

			LoadUser();
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

		private async Task UploadImage(UIImage image)
		{
			var imageFilePath = await ImageFilesManagementHelper.SharedInstance.SaveImageToTemporaryFilePng(image);

			var fileUploadRequest = new FileUploadRequest ();

			fileUploadRequest.ProgressUpdated += (int progressPercentage) => Debug.WriteLine ("Upload progress: " + progressPercentage);

			fileUploadRequest.Success += async (string remoteFileUrl) => 
			{
				Debug.WriteLine ("Upload complete - file URL: " + remoteFileUrl);
				ImageFilesManagementHelper.SharedInstance.RemoveTemporaryFile(imageFilePath);

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
				ImageFilesManagementHelper.SharedInstance.RemoveTemporaryFile(imageFilePath);
			};


			fileUploadRequest.Perform(imageFilePath);
		}
						
		void LoadUser()
		{
			user = UserManager.LoggedInUser();
            
            displayNameLabel.Text = user.DisplayName;
		}
	}
}
