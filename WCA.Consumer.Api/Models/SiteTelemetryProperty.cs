﻿using System.Collections.Generic;

namespace WCA.Consumer.Api.Models
{
    public class SiteTelemetryProperty
    {
        public IList<string> Objects { get; set; } = new List<string>();
        public IList<string> Locations { get; set; } = new List<string>();
    }
}