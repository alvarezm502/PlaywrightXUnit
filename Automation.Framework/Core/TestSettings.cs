using System;
using System.Collections.Generic;
using System.Text;

namespace Automation.Framework.Core
{
    public class TestSettings
    {
        public string BaseUrl { get; set; }
        public string[] Args { get; set; }
        public float Timeout { get; set; }
        public bool Headless { get; set; }
        public bool DevTools { get; set; }
        public DriverType DriverType { get; set; }
        public string Mpw {  get; set; }
    }

    public enum DriverType
    {
        Chromium,
        Firefox,
        Edge,
        Webkit
    }
}
