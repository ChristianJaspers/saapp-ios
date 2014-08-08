using System;
using System.Drawing;
using MonoTouch.UIKit;
using XValidator;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.iOS
{
	public partial class ArgumentFormViewController : BaseUIViewController
	{
        public XForm<UITextView> Validator;
        
        public Argument Argument;
        
		public ArgumentFormViewController (IntPtr handle) : base (handle)
		{
		}
        
        #region Lifecycle
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            InitValidator();

			labelFeature.Text = I18n.ArgumentAddFeature;
			labelBenefit.Text = I18n.ArgumentAddBenefit;

			// TODO
			// labelEarnXpForFeature.Text = I18n.FeatureAddEarnXp;
			// labelEarnXpForBenefit.Text = I18n.BenefitAddEarnXp;
            
            Feature = Argument.Feature;
            Benefit = Argument.Benefit;
            
            SubscribeEvents();

			AddKeyboardAccessoryView(fieldFeature);
			AddKeyboardAccessoryView(fieldBenefit);
        }
			
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            
            UnsubscribeEvents();
        }

		private void AddKeyboardAccessoryView(UITextView textView)
		{
			int toolbarHeight = 44;
			int toolbarWidth = (int)View.Frame.Width;

			var toolbar = new UIToolbar(new Rectangle(0, 0, toolbarWidth, toolbarHeight));
			toolbar.TintColor = UIColor.Black;
			toolbar.BarStyle = UIBarStyle.Default;

			toolbar.Items = new UIBarButtonItem[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
					textView.ResignFirstResponder();
				})
			};

			textView.KeyboardAppearance = UIKeyboardAppearance.Light;
			textView.InputAccessoryView = toolbar;
		}

        
        #endregion
        
        public string Feature
        {
            get { return fieldFeature.Text; }
            set { fieldFeature.Text = value; }
        }
        
        public string Benefit
        {
            get { return fieldBenefit.Text; }
            set { fieldBenefit.Text = value; }
        }
        
        #region Validation

        void InitValidator()
        {
            var validatorRequired = new XValidatorRequired {
                Message = I18n.ValidationRequired
            };

            Validator = new XForm<UITextView> {
                Inputs = new [] {
                    new XUITextViewField {
                        Name = I18n.FieldFeature,
                        FieldView = fieldFeature,
                        Validators = new [] { validatorRequired },
                    },
                    new XUITextViewField {
                        Name = I18n.FieldBenefit,
                        FieldView = fieldBenefit,
                        Validators = new [] { validatorRequired },
                    }
                }
            };
        }
        
        #endregion
        
        #region Field events
        
        void DidStartEditing(object sender, EventArgs e)
        {   
            scrollViewContainer.SetContentOffset(new PointF(0,((UIView)sender).Frame.Y), true);
        }
        
        void DidEndEditing(object sender, EventArgs e)
        {
            Argument.Feature = Feature;
            Argument.Benefit = Benefit;
            
            scrollViewContainer.SetContentOffset(PointF.Empty, true);
        }

        void SubscribeEvents()
        {
            fieldFeature.Started += DidStartEditing;
            fieldFeature.Ended += DidEndEditing;
            fieldBenefit.Started += DidStartEditing;
            fieldBenefit.Ended += DidEndEditing;
        }

        void UnsubscribeEvents()
        {
            fieldFeature.Started -= DidStartEditing;
            fieldFeature.Ended -= DidEndEditing;
            fieldBenefit.Started -= DidStartEditing;
            fieldBenefit.Ended -= DidEndEditing;
        }
        
        #endregion
	}
}
