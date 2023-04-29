using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entity
{
    public class Conversation
    {
        public int ConversationId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
        public bool DeletedBySender { get; set; } = false;
        public bool DeletedByReceiver { get; set; } = false;
        public ICollection<Message> Messages { get; set; }
    }
}