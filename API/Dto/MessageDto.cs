using API.Entity;

namespace API.Dto
{
    public class MessageDto
    {
       public int Id { get; set; }
       public int ConversationId { get; set; }
       public int SenderId { get; set; }
       public string Content { get; set; }
       public DateTime SentOn { get; set; }
    }
}
