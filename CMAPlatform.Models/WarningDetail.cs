namespace CMAPlatform.Models
{
    public class WarningDetail
    {
        public string identifier { get; set; }
        public string sender { get; set; }
        public string senderCode { get; set; }
        public string sendTime { get; set; }
        public string effective { get; set; }
        public string msgType { get; set; }
        public string eventType { get; set; }
        public string severity { get; set; }
        public string headline { get; set; }
        public string description { get; set; }
        public string lon { get; set; }
        public string lat { get; set; }
    }
}