namespace API.Entity
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; } = DateTime.Now;
        public DateTime? ReadOn { get; set; }
        public bool DeletedBySender { get; set; } = false;
        public bool DeletedByReceiver { get; set; } = false;
    }
}