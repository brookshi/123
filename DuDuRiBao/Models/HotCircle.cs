﻿using Brook.DuDuRiBao.Utils;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Media;

namespace Brook.DuDuRiBao.Models
{
    public class HotCircle
    {
        public string Thumbnail { get; set; }

        public string Id { get; set; }

        public string Articles { get; set; }

        public string Fans { get; set; }

        public Brush BackgroundBrush { get; set; }

        public string Name { get; set; }

        public void Adjust()
        {
            if(string.IsNullOrEmpty(Thumbnail) && !string.IsNullOrEmpty(Name))
            {
                Thumbnail = Name[0].ToString();
            }
            BackgroundBrush = ColorUtil.GetBrushByCircleId(int.Parse(Id));
        }

        public HotCircle Clone()
        {
            return new HotCircle()
            {
                Thumbnail = Thumbnail,
                Id = Id,
                Articles = Articles,
                Fans = Fans,
                BackgroundBrush = BackgroundBrush,
                Name = Name
            };
        }
    }
}
