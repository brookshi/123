﻿using System.Collections.Generic;

namespace Brook.DuDuRiBao.Models
{
    public class Others
    {
        public int color { get; set; }
        public string thumbnail { get; set; }
        public string description { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class RootObject
    {
        public int limit { get; set; }
        public List<object> subscribed { get; set; }
        public List<Others> others { get; set; }
    }
}
