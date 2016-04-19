﻿using System.Collections.Generic;

namespace Brook.DuDuRiBao.Models
{
    public class Theme
    {
        public int id { get; set; }
        public bool subscribed { get; set; }
        public string name { get; set; }
    }

    public class Story
    {
        public List<string> images { get; set; }
        public int type { get; set; }
        public int id { get; set; }
        public string ga_prefix { get; set; }
        public string title { get; set; }
        public Theme theme { get; set; }
        public bool? multipic { get; set; }
    }

    public class TopStory
    {
        public string image { get; set; }
        public int type { get; set; }
        public int id { get; set; }
        public string ga_prefix { get; set; }
        public string title { get; set; }
    }

    public class MainData
    {
        public string date { get; set; }
        public List<Story> stories { get; set; }
        public List<TopStory> top_stories { get; set; }
    }

    public class Favorites
    {
        public int count { get; set; }
        public List<Story> stories { get; set; }
        public int? last_time { get; set; } = null;
    }
}
