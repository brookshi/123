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

using Windows.UI.Xaml.Controls;

namespace Brook.DuDuRiBao.Common
{
    public class PageBase : Page
    {
        protected T GetVM<T>() where T : ViewModelBase { return DataContext as T; }
        protected bool _canUseCached = false;

        public PageBase()
        {
            _canUseCached = false;
        }

        protected void Initalize()
        {
            GetVM<ViewModelBase>().Init();
        }

        protected bool IsUsingCachedWhenNavigate()
        {
            var isUsingCached = NavigationCacheMode != Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled &&
                _canUseCached;

            _canUseCached = true;

            return isUsingCached;
        }
    }
}
