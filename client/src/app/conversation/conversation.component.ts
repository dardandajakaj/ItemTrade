import { Component, OnInit } from '@angular/core';
import { Conversation } from '../_Models/Conversation';
import { ChatService } from '../_Services/chat.service';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { ProductService } from '../_Services/product.service';
import { Message } from '../_Models/Message';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';


@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.css']
})
export class ConversationComponent implements OnInit {
  messages : Message[] = null;
  conversations$ = new Observable<Conversation[]>;
  currentConversation : Conversation;
  user = JSON.parse(localStorage.getItem('user'));
  faPaperPlane = faPaperPlane;
  constructor(private chatService: ChatService, private productService : ProductService, private toastR: ToastrService) { }

  ngOnInit(): void {
    this.getAllConversations()
  }

  getProductConversations(productId: number) {
    this.conversations$ = this.chatService.getProductConversations(productId);
  }

  getAllConversations(){
    this.conversations$ = this.chatService.getUserConversations(this.user['id']);
  }
  openConversation(conversationId: number) {
    this.toastR.info("Navigate to " + conversationId);
  }

  selectChat(id: number){
    this.messages = null;
    this.conversations$.subscribe(data => this.currentConversation = data.find(x => x.conversationId == id))
    this.chatService.getMessages(id).subscribe(response => {
      this.messages = response;
    })
  }
}
