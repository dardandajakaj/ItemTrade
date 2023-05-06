using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entity;

namespace API.Dto
{
    public class ConversationDto
    {
        public int ConversationId { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public int SenderId { get; set; }
        public string Sender { get; set; }
        public int ReceiverId { get; set; }
        public string Receiver { get; set; }
    }
}