﻿using Brook.DuDuRiBao.Common;
using Brook.DuDuRiBao.Utils;
using Brook.DuDuRiBao.ViewModels;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using LLQ;
using Brook.DuDuRiBao.Events;
using LLM;
using System;
using Windows.UI.Xaml.Media;

namespace Brook.DuDuRiBao.Pages
{
    public sealed partial class MainContentPage : Page
    {
        public MainContentViewModel VM { get { return DataContext as MainContentViewModel; } }

        public MainContentPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            LLQNotifier.Default.Register(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            WebViewUtil.AddWebViewWithBinding(MainContent, VM, "MainHtmlContent");

            if (Config.UIStatus == AppUIStatus.List)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            CurrentUIStatus = Config.UIStatus;
            VM.RequestStoryData();
            LLQNotifier.Default.Notify(new OpenNewStoryEvent());
        }

        public Visibility ToolBarVisibility { get { return Config.IsSinglePageStatus(CurrentUIStatus) ? Visibility.Visible : Visibility.Collapsed; } }

        public AppUIStatus CurrentUIStatus { get; set; }

        [SubscriberCallback(typeof(StoryEvent))]
        private void Subscriber(StoryEvent param)
        {
            switch (param.Type)
            {
                case StoryEventType.Comment:
                    if (Config.IsSinglePageStatus(CurrentUIStatus))
                    {
                        Frame rootFrame = App.GetWindowFrame();
                        if (rootFrame == null)
                            return;

                        rootFrame.Navigate(typeof(CommentPage));
                    }
                    break;
                case StoryEventType.ShareToWeiBo:
                    if (WeiboSharePopup.IsOpen)
                        break;

                    WeiboSharePopup.IsOpen = true;
                    PostMsg.Text = string.Format($"{VM.MainHtmlContent.Title} {VM.MainHtmlContent.Share_Url}");
                    Animator.Use(AnimationType.ZoomInDown).SetDuration(TimeSpan.FromMilliseconds(800)).PlayOn(WeiboSharePopup, () =>
                    {
                        var transform = (CompositeTransform)PrepareTransform(WeiboSharePopup, typeof(CompositeTransform));
                        transform.CenterX = transform.CenterY = 0;
                    });
                    break;
            }
        }

        private void CloseWeiBoShareDlg(object sender, RoutedEventArgs e)
        {
            Animator.Use(AnimationType.Hinge).PlayOn(WeiboSharePopup, () =>
            {
                WeiboSharePopup.IsOpen = false;
                var transform = (CompositeTransform)PrepareTransform(WeiboSharePopup, typeof(CompositeTransform));
                transform.Rotation = 0;
                transform.TranslateX = transform.TranslateY = 0;
            });
        }

        private async void ShareWeiBo(object sender, RoutedEventArgs e)
        {
            if (VM == null || VM.MainHtmlContent == null)
                return;

            Animator.Use(AnimationType.ZoomOutUp).SetDuration(TimeSpan.FromMilliseconds(800)).PlayOn(WeiboSharePopup, () =>
            {
                WeiboSharePopup.IsOpen = false;
                var transform = (CompositeTransform)PrepareTransform(WeiboSharePopup, typeof(CompositeTransform));
                transform.CenterX = transform.CenterY = 0;
                transform.ScaleX = transform.ScaleY = 1;
                transform.TranslateX = transform.TranslateY = 0;
                WeiboSharePopup.Opacity = 1;
            });

            WeiboSDKForWinRT.SdkNetEngine engine = new WeiboSDKForWinRT.SdkNetEngine();
            var response = await engine.RequestCmd(WeiboSDKForWinRT.SdkRequestType.POST_MESSAGE, new WeiboSDKForWinRT.CmdPostMessage() { Status = PostMsg.Text });
            if (response.errCode == WeiboSDKForWinRT.SdkErrCode.SUCCESS)
                PopupMessage.DisplayMessageInRes("ShareSuccess");
            else
                PopupMessage.DisplayMessageInRes("ShareFailed");
        }
    }
}
