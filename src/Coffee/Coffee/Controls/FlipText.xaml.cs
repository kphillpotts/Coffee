using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Coffee.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlipText : Grid
    {
        public static readonly BindableProperty TextStyleProperty =
     BindableProperty.Create(nameof(TextStyle),
         typeof(Style),
         typeof(FlipText),
         default, propertyChanged: OnTextStyleChanged);
        public Style TextStyle
        {
            get
            {
                return (Style)GetValue(TextStyleProperty);
            }

            set
            {
                SetValue(TextStyleProperty, value);
            }
        }

        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var control = (FlipText)bindable;

                var value = (Style)newValue;

                control.ApplyTextStyle(value);
            }
            catch (Exception ex)
            {
                // TODO: Handle exception.
            }
        }

        void ApplyTextStyle(Style value)
        {
            CurrentLabel.Style = value;
            NextLabel.Style = value;
        }

        public Point AnimationOffset { get; set; }


        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(FlipText), default(string), propertyChanged: OnTextChanged);
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        public Label Current { get; set; }
        public Label Next { get; set; }
        public uint AnimationSpeed { get; internal set; }

        static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var control = (FlipText)bindable;

                var value = (string)newValue;

                control.ApplyText((string)oldValue, value);
            }
            catch (Exception ex)
            {
                // TODO: Handle exception.
            }
        }
        double distance = 20;

        async void ApplyText(string oldValue, string newValue)
        {
            // update the labels
            Current.Text = oldValue;
            Current.TranslationY = 0;
            Current.TranslationX = 0;
            Current.Opacity = 1;

            // set the starting positions
            Current.TranslationY = 0;
            _ = Current.TranslateTo(-AnimationOffset.X, -AnimationOffset.Y, AnimationSpeed);
            _ = Current.FadeTo(0, AnimationSpeed);

            // animate in the next label
            Next.Text = newValue;
            Next.TranslationY = AnimationOffset.Y;
            Next.TranslationX = AnimationOffset.X;
            Next.Opacity = 0;
            _ = Next.TranslateTo(0, 0, AnimationSpeed);
            await Next.FadeTo(1, AnimationSpeed);

            // recycle the views
            Current = NextLabel;
            Next = CurrentLabel;
        }

        public FlipText()
        {
            InitializeComponent();
            Current = CurrentLabel;
            Next = NextLabel;

        }
    }
}