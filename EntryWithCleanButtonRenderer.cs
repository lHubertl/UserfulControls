using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinTest.Droid;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(Entry), typeof(EntryWithCleanButtonRenderer))]
namespace XamarinTest.Droid
{
    class EntryWithCleanButtonRenderer : EntryRenderer
    {
        private bool isCleanButtonAlreadyConfigured;
        private Drawable rightDrawable;

        public EntryWithCleanButtonRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = null;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Entry.TextProperty.PropertyName)
            {
                if (string.IsNullOrEmpty(Control.Text))
                {
                    ConfigureCleanButtonToControl(false);
                }
                else
                {
                    ConfigureCleanButtonToControl(true);
                }
            }

            if (e.PropertyName == VisualElement.IsFocusedProperty.PropertyName)
            {
                if (Element.IsFocused == false)
                {
                    ConfigureCleanButtonToControl(false);
                }
                else
                {
                    if (string.IsNullOrEmpty(Control.Text))
                    {
                        ConfigureCleanButtonToControl(false);
                    }
                    else
                    {
                        ConfigureCleanButtonToControl(true);
                    }
                }
            }
        }

        private void ConfigureCleanButtonToControl(bool showCleanButton)
        {
            if (isCleanButtonAlreadyConfigured && showCleanButton)
            {
                return;
            }

            if (rightDrawable == null)
            {
                rightDrawable = Resources.GetDrawable(Android.Resource.Drawable.IcMenuCloseClearCancel, null);
            }

            if (showCleanButton)
            {
                Control.SetCompoundDrawablesRelativeWithIntrinsicBounds(null, null, rightDrawable, null);

                // To unregister event if it was registered
                Control.Touch -= Control_Touch;
                Control.Touch += Control_Touch;
            }
            else
            {
                Control.SetCompoundDrawablesRelativeWithIntrinsicBounds(null, null, null, null);
                Control.Touch -= Control_Touch;
            }

            isCleanButtonAlreadyConfigured = showCleanButton;
        }

        private void Control_Touch(object sender, TouchEventArgs e)
        {
            int fuzz = 10;
            var v = sender as View;
            var handled = false;

            if (v != null)
            {
                if (e.Event.Action == MotionEventActions.Down)
                {
                    Element.Focus();
                    handled = true;

                    var x = e.Event.GetX();
                    var y = e.Event.GetY();
                    var bound = rightDrawable.Bounds;

                    if (x >= v.Right - bound.Width() - fuzz &&
                        x <= v.Right - v.PaddingRight + fuzz &&
                        y >= v.PaddingTop - fuzz &&
                        y <= v.Height - v.PaddingBottom + fuzz)
                    {
                        Element.Text = "";
                    }
                }
                else if (e.Event.Action == MotionEventActions.Up)
                {
                    handled = true;
                }
            }

            e.Handled = handled;
        }
    }
}