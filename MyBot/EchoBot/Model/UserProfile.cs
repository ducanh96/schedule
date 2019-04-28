﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Model
{
    public class UserProfile
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Date { get; set; }
        public bool IsStartConverstation { get; set; } = false;
        public bool IsStartTransition { get; set; } = false;
    }
}
