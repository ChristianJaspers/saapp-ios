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
using MBProgressHUD;

namespace BetterSalesman.iOS
{
	partial class ProfileViewController : BaseUIViewController
	{
		private ImagePickerPresenter imagePickerPresenter;
		private UIImage cachedPickedImage;
		private User user;

		public ProfileViewController(IntPtr handle) : base (handle)
		{
			imagePickerPresenter = new ImagePickerPresenter ();
			imagePickerPresenter.FinishedPicking += (bool didPickAnImage, UIImage pickedImage) => 
			{
				Task.Run(async () =>
				{
					if (didPickAnImage)
					{
					  await UploadImage(pickedImage);
					}
				});
			};
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			BackButton.TouchUpInside += (sender, e) =>
			{
				DismissViewController (true, null);
			};

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
			cachedPickedImage = image;

			var imageFilePath = await ImageFilesManagementHelper.SharedInstance.SaveImageToTemporaryFilePng(image);

			var fileUploadRequest = new FileUploadRequest();

			fileUploadRequest.ProgressUpdated += (int progressPercentage) => 
			{
				Debug.WriteLine ("Upload progress: " + progressPercentage);

				SetHudProgress(progressPercentage / 100.0f);
			};

			fileUploadRequest.Success += async (string remoteFileUrl) => 
			{
				ImageFilesManagementHelper.SharedInstance.RemoveTemporaryFile(imageFilePath);

				InvokeOnMainThread(() =>
				{
					Debug.WriteLine ("Upload complete - file URL: " + remoteFileUrl);
					ProfileImageView.Image = cachedPickedImage;
					cachedPickedImage = null;
				});

				HideHud();
			};

			fileUploadRequest.Failure += (int errorCode, string errorMessage) =>
			{
				ImageFilesManagementHelper.SharedInstance.RemoveTemporaryFile(imageFilePath);
				cachedPickedImage = null;

				InvokeOnMainThread(() => ShowAlert(errorMessage));

				HideHud();
			};

			ShowHud("Updating profile picture", MBProgressHUDMode.Indeterminate);
			fileUploadRequest.Perform(imageFilePath);
		}
						
		void LoadUser()
		{
			user = UserManager.LoggedInUser();
            
            displayNameLabel.Text = user.DisplayName;
		}
	}
}
