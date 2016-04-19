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


namespace Brook.DuDuRiBao.Utils
{
    public static class Misc
    {
        public const int Group_Name_Type = -100;

        public const int Default_Category_Id = -1;

        public const int Favorite_Category_Id = -2;

        public const int Page_Count = 30;

        public const int Unvalid_Image_Id = -1;

        public static bool IsGroupItem(int type)
        {
            return Group_Name_Type == type;
        }
    }
}
