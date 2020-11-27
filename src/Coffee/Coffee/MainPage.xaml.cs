using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Coffee
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        AnimationStateMachine animStates = new AnimationStateMachine();

        enum PageStates
        {
            Closed,
            Peek,
            Open
        }

        private double pageHeight;
        private double thumbHeight = 25;
        private double openThreshold = -300; // should be 1/3 of the screen

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            pageHeight = height;
            SetupStates();

        }


        private void SetupStates()
        {
            animStates = new AnimationStateMachine();

            animStates.Add(PageStates.Closed, new ViewTransition[] {
                new ViewTransition(FrontCard, AnimationType.TranslationY, 0),
                new ViewTransition(Thumb,  AnimationType.Opacity, 0),
            });

            animStates.Add(PageStates.Peek, new ViewTransition[] {
                new ViewTransition(FrontCard, AnimationType.TranslationY, -100),
                new ViewTransition(Thumb,  AnimationType.Opacity, 1),
            });

            animStates.Add(PageStates.Open, new ViewTransition[] {
                new ViewTransition(FrontCard, AnimationType.TranslationY, -(this.Height - 25)),
                new ViewTransition(Thumb,  AnimationType.Opacity, 1),
            });
        }

        double lastVelocity;

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    lastVelocity = e.TotalY;
                    FrontCard.TranslationY += e.TotalY;
                    break;
                case GestureStatus.Completed:
                    if (lastVelocity < -10)
                        animStates.Go(PageStates.Open);
                    else if (lastVelocity > 10)
                        animStates.Go(PageStates.Peek);
                    else
                    {
                        if (FrontCard.TranslationY < openThreshold)
                            animStates.Go(PageStates.Open);
                        else
                            animStates.Go(PageStates.Peek);
                    }
                    break;
            }
        }

        private void AddToBag_Clicked(object sender, EventArgs e)
        {
            animStates.Go(PageStates.Peek);
        }

        private void SwipeUp_Swiped(object sender, SwipedEventArgs e)
        {
            animStates.Go(PageStates.Open);
        }

        private void SwipeDown_Swiped(object sender, SwipedEventArgs e)
        {
            animStates.Go(PageStates.Peek);
        }
    }
}
