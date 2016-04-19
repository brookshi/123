﻿using System;
using System.Collections.Generic;
using System.Threading;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Brook.DuDuRiBao.Utils
{
    public class PopupMessage
    {
        private const int AnimDuration = 200;
        private const int PopupStayTime = 2000;
        private const int PopupPosition = -100;
        private const int TextPadding_LR = 20;
        private const int TextPadding_TB = 5;

        private static Popup _popup = new Popup();

        private static TextBlock _msgText = new TextBlock();

        private readonly static Queue<string> _msgQueue = new Queue<string>();

        private static int _popupState = 0;

        static PopupMessage()
        {
            _msgText.Padding = new Thickness(TextPadding_LR, TextPadding_TB, TextPadding_LR, TextPadding_TB);
            _msgText.Foreground = new SolidColorBrush(Colors.White);
            _msgText.HorizontalAlignment = HorizontalAlignment.Center;

            Border border = new Border();
            border.CornerRadius = new CornerRadius(5);
            border.MinWidth = 100;
            border.Background = new SolidColorBrush(Colors.Black);
            border.Child = _msgText;

            _popup.Child = border;
            _popup.RenderTransform = new TranslateTransform();
            _popup.IsOpen = true;
            UpdatePosition();
        }

        public static void DisplayMessageInRes(string resId)
        {
            DisplayMessage(StringUtil.GetString(resId));
        }

        public static void DisplayMessage(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return;

            lock(_msgQueue)
            {
                if (!_msgQueue.Contains(msg))
                    _msgQueue.Enqueue(msg);
            }

            Popup();
        }

        private static void Popup()
        {
            if (_msgQueue.Count == 0)
                return;

            if (Interlocked.CompareExchange(ref _popupState, 1, 0) == 1)
                return;

            string msg;
            lock(_msgQueue)
            {
                msg = _msgQueue.Dequeue();
            }

            _msgText.Text = msg;

            UpdatePosition();

            PopupWithAnim();
        }

        private static void PopupWithAnim()
        {
            Storyboard story = new Storyboard();
            DoubleAnimation popupUpAnim = CreateDoubleAnimation(_popup.RenderTransform, "Y", new BackEase() { Amplitude = 0.6, EasingMode = EasingMode.EaseOut }, PopupPosition, AnimDuration);
            story.Children.Add(popupUpAnim);
            story.Completed +=(s, e)=>
            {
                story = new Storyboard();
                DoubleAnimation popupDownAnim = CreateDoubleAnimation(_popup.RenderTransform, "Y", new BackEase() { Amplitude = 0.6, EasingMode = EasingMode.EaseIn }, 0, AnimDuration);
                popupDownAnim.BeginTime = TimeSpan.FromMilliseconds(PopupStayTime);
                story.Children.Add(popupDownAnim);

                story.Completed += (s1, e1) =>
                {
                    story.Stop();
                    Interlocked.Decrement(ref _popupState);
                    Popup();
                };
                story.Begin();
            };

            story.Begin();
        }

        private static DoubleAnimation CreateDoubleAnimation(DependencyObject target, string propertyPath, EasingFunctionBase easingFunc, double to, double duration)
        {
            var anim = new DoubleAnimation()
            {
                To = to,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                EasingFunction = easingFunc
            };
            Storyboard.SetTarget(anim, target);
            Storyboard.SetTargetProperty(anim, propertyPath);
            return anim;
        }

        private static void UpdatePosition()
        {
            if (string.IsNullOrEmpty(_msgText.Text))
                return;

            MeasureText();

            var window = Window.Current.CoreWindow;
            _popup.HorizontalOffset = window.Bounds.Width / 2 - _msgText.ActualWidth / 2;
            _popup.VerticalOffset = window.Bounds.Height;
        }

        private static void MeasureText()
        {
            var size = new Size(1000, 1000);
            _msgText.Measure(size);
            _msgText.Arrange(new Rect(new Point(0, 0), size));
        }
    }
}
