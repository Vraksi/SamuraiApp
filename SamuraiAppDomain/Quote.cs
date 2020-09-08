using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiAppDomain
{

    public class Quote
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Samurai Samurai { get; set; }
        public int SamuraiId { get; set; }
    }
}
