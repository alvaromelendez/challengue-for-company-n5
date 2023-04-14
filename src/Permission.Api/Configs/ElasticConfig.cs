namespace Permissions.Api.Configs
{
    public class ElasticConfig
    {
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableLogs { get; set; }
        public Dictionary<string, string> Indexes { get; set; }
    }
}
