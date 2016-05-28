﻿#region License
//   Copyright 2015 Brook Shi
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using Brook.DuDuRiBao.Authorization;
using Brook.DuDuRiBao.Common;
using Brook.DuDuRiBao.Events;
using Brook.DuDuRiBao.Models;
using Brook.DuDuRiBao.Utils;
using LLQ;

namespace Brook.DuDuRiBao.ViewModels
{
    public class MainContentViewModel : ViewModelBase
    {
        private MainContent _mainHtmlContent;
        public MainContent MainHtmlContent
        {
            get { return _mainHtmlContent; }
            set
            {
                if (value != _mainHtmlContent)
                {
                    _mainHtmlContent = value;
                    Notify("MainHtmlContent");
                }
            }
        }

        private bool _isRefreshContent = false;
        public bool IsRefreshContent
        {
            get { return _isRefreshContent; }
            set
            {
                if (value != _isRefreshContent)
                {
                    _isRefreshContent = value;
                    Notify("IsRefreshContent");
                }
            }
        }

        public MainContentViewModel()
        {
            StoryExclusiveSubscriber.Instance.VM = this;
        }

        static MainContentViewModel()
        {
            LLQNotifier.Default.Register(StoryExclusiveSubscriber.Instance); 
        }

        public string Title
        {
            get { return StringUtil.GetString("ContentTitle"); }
        }

        public async void RequestMainContent()
        {
            if (string.IsNullOrEmpty(CurrentStoryId))
                return;

            IsRefreshContent = true;
            var content = await DataRequester.RequestStoryContent(CurrentStoryId);
            if (content != null)
            {
                Html.ArrangeMainContent(content);
                MainHtmlContent = content;
                LLQNotifier.Default.Notify(new ShareEvent() { ShareUrl = content.Share_Url });
            }
            IsRefreshContent = false;
        }

        public async void RequestStoryExtraInfo()
        {
            var currentStoryExtraInfo = await DataRequester.RequestStoryExtraInfo(CurrentStoryId);
            CurrentStoryExtraInfo = currentStoryExtraInfo ?? DefaultStoryExtraInfo;
            LLQNotifier.Default.Notify(new StoryExtraEvent() { StoryExtraInfo = CurrentStoryExtraInfo });
        }

        public void RequestStoryData()
        {
            RequestMainContent();

            RequestStoryExtraInfo();
        }

        internal class StoryExclusiveSubscriber
        {
            internal static StoryExclusiveSubscriber Instance = new StoryExclusiveSubscriber();
            internal MainContentViewModel VM { get; set; }

            [SubscriberCallback(typeof(StoryEvent))]
            private void Subscriber(StoryEvent param)
            {
                switch (param.Type)
                {
                    case StoryEventType.Fav:
                        DataRequester.SetFavoriteStory(CurrentStoryId, param.IsChecked);
                        break;
                    case StoryEventType.Like:
                        DataRequester.SetStoryLike(CurrentStoryId, param.IsChecked);
                        break;
                    case StoryEventType.Night:
                        if(VM != null)
                            VM.RequestMainContent();
                        break;
                }
            }
        }
    }
}
