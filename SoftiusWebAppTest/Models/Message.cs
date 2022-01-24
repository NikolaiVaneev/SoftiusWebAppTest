using System;

namespace SoftiusWebAppTest.Models
{
    public class Message
    {
        public DateTime MessageSendingTime { get; set; } = DateTime.Now;
        public Student From { get; set; }
        public Student To { get; set; }
        public string Content { get; set; }
    }
}
