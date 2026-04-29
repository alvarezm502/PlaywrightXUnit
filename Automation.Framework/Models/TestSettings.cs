namespace Automation.Framework.Models
{
    public class TestSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string[] Args { get; set; } = [];
        public float Timeout { get; set; }
        public bool Headless { get; set; }
        public bool DevTools { get; set; }
        public DriverType DriverType { get; set; }
        public string Mpw {  get; set; } = string.Empty;
    }

    public enum DriverType
    {
        Chromium,
        Firefox,
        Edge,
        Webkit
    }
}
