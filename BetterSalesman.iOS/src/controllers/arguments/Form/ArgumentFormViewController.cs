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
        
		public ArgumentFormViewController (IntPtr handle) : base (handle)
		{
		}
        
        #region Lifecycle
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            fieldFeature.Text = feature;
            fieldBenefit.Text = benefit;
            
            InitValidator();
            
            SubscribeEvents();
        }
        
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            
            UnsubscribeEvents();
        }
        
        #endregion
        
        string feature;
        public string Feature
        {
            get { return fieldFeature.Text; }
            set { feature = value; }
        }
        
        string benefit;
        public string Benefit
        {
            get { return fieldBenefit.Text; }
            set { benefit = value; }
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
