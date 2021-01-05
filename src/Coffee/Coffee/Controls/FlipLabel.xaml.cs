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
    public partial class FlipLabel : StackLayout
    {



        #region AnimationSpeed
        public static readonly BindableProperty AnimationSpeedProperty = BindableProperty.Create(nameof(AnimationSpeed), typeof(uint), typeof(FlipLabel), propertyChanged: (obj, old, newV) =>
        {
            var me = obj as FlipLabel;
            if (newV != null && !(newV is uint)) return;
            var oldAnimationSpeed = (uint)old;
            var newAnimationSpeed = (uint)newV;
            me?.AnimationSpeedChanged(oldAnimationSpeed, newAnimationSpeed);
        });

        private void AnimationSpeedChanged(uint oldAnimationSpeed, uint newAnimationSpeed)
        {
            foreach (var item in this.Children)
            {
                ((FlipText)item).AnimationSpeed = newAnimationSpeed;
            }
        }

        /// <summary>
        /// A bindable property
        /// </summary>
        public uint AnimationSpeed
        {
            get => (uint)GetValue(AnimationSpeedProperty);
            set => SetValue(AnimationSpeedProperty, value);
        }
        #endregion




        #region Text
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(FlipLabel), propertyChanged: (obj, old, newV) =>
        {
            var me = obj as FlipLabel;
            if (newV != null && !(newV is string)) return;
            var oldText = (string)old;
            var newText = (string)newV;
            me?.TextChanged(oldText, newText);
        });

        private async void TextChanged(string oldText, string newText)
        {
            if (newText == null) return;

            Task[] tasks = new Task[NumberOfCharacters];
            Random rnd = new Random();

            // assign all the characters
            for (int i = 0; i < NumberOfCharacters; i++)
            {
                // create a task for updating each character
                var currentchar = i;
                tasks[i] = Task.Run(async () =>
                {
                    await Task.Delay(rnd.Next((int)(AnimationSpeed * .5), (int)AnimationSpeed));

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // get the text for the character
                        string charText = newText[currentchar].ToString();
                        // update the text
                        ((FlipText)this.Children[currentchar]).Text = charText;
                    });
                });
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// A bindable property
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion



        #region NumberOfCharacters
        public static readonly BindableProperty NumberOfCharactersProperty = BindableProperty.Create(nameof(NumberOfCharacters), typeof(int), typeof(FlipLabel), propertyChanged: (obj, old, newV) =>
        {
            var me = obj as FlipLabel;
            if (newV != null && !(newV is int)) return;
            var oldNumberOfCharacters = (int)old;
            var newNumberOfCharacters = (int)newV;
            me?.NumberOfCharactersChanged(oldNumberOfCharacters, newNumberOfCharacters);
        });

        private void NumberOfCharactersChanged(int oldNumberOfCharacters, int newNumberOfCharacters)
        {
            CreateChar();
        }

        /// <summary>
        /// A bindable property
        /// </summary>
        public int NumberOfCharacters
        {
            get => (int)GetValue(NumberOfCharactersProperty);
            set => SetValue(NumberOfCharactersProperty, value);
        }
        #endregion



        #region TextStyle
        public static readonly BindableProperty TextStyleProperty = BindableProperty.Create(nameof(TextStyle), typeof(Style), typeof(FlipLabel), propertyChanged: (obj, old, newV) =>
        {
            var me = obj as FlipLabel;
            if (newV != null && !(newV is Style)) return;
            var oldTextStyle = (Style)old;
            var newTextStyle = (Style)newV;
            me?.TextStyleChanged(oldTextStyle, newTextStyle);
        });

        private void TextStyleChanged(Style oldTextStyle, Style newTextStyle)
        {
            foreach (var item in this.Children)
            {       
                ((FlipText)item).TextStyle = newTextStyle;
            }

        }

        /// <summary>
        /// A bindable property
        /// </summary>
        public Style TextStyle
        {
            get => (Style)GetValue(TextStyleProperty);
            set => SetValue(TextStyleProperty, value);
        }
        #endregion



        #region AnimationOffset
        public static readonly BindableProperty AnimationOffsetProperty = BindableProperty.Create(nameof(AnimationOffset), typeof(Point), typeof(FlipLabel), propertyChanged: (obj, old, newV) =>
        {
            var me = obj as FlipLabel;
            if (newV != null && !(newV is Point)) return;
            var oldAnimationOffset = (Point)old;
            var newAnimationOffset = (Point)newV;
            me?.AnimationOffsetChanged(oldAnimationOffset, newAnimationOffset);
        });

        private void AnimationOffsetChanged(Point oldAnimationOffset, Point newAnimationOffset)
        {
            foreach (var item in this.Children)
            {
                ((FlipText)item).AnimationOffset = newAnimationOffset;
            }
        }

        /// <summary>
        /// A bindable property
        /// </summary>
        public Point AnimationOffset
        {
            get => (Point)GetValue(AnimationOffsetProperty);
            set => SetValue(AnimationOffsetProperty, value);
        }
        #endregion





        public FlipLabel()
        {
            InitializeComponent();
            CreateChar();
        }

        void CreateChar()
        {
            this.Children.Clear();

            // add in the characters
            for (int i = 0; i < NumberOfCharacters; i++)
            {
                var character = new FlipText();
                character.Text = "0";
                character.AnimationOffset = new Point(0, 20);
                character.AnimationSpeed = this.AnimationSpeed;
                this.Children.Add(character);
            }
        }
    }
}