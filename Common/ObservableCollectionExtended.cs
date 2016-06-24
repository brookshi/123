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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Brook.DuDuRiBao.Common
{
    public class ObservableCollectionExtended<T> : ObservableCollection<T>
    {
        public void AddRange(IEnumerable<T> dataToAdd)
        {
            if (dataToAdd == null || dataToAdd.Count() == 0)
                return;

            CheckReentrancy();
            int startingIndex = Count;

            foreach (var data in dataToAdd)
            {
                Items.Add(data);
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

            var changedItems = new List<T>(dataToAdd);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startingIndex));
        }
    }
}
