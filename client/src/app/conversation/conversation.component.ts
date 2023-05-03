import { Component, OnDestroy, OnInit } from '@angular/core';
import { Conversation } from '../_Models/Conversation';
import { ChatService } from '../_Services/chat.service';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { Message } from '../_Models/Message';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { MessageDto } from '../_Models/MessageDto';


@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.css']
})
export class ConversationComponent implements OnInit, OnDestroy {
  // messages : Message[] = null;
  conversations$ = new Observable<Conversation[]>;
  currentConversation : Conversation;
  user = JSON.parse(localStorage.getItem('user'));
  faPaperPlane = faPaperPlane;
  chatter: string = "";
  msg: string;

  constructor(public chatService: ChatService, private toastR: ToastrService) { }

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
    // this.messages = null;
    this.msg = "";
    this.conversations$.subscribe(data => this.currentConversation = data.find(x => x.conversationId == id))
    console.log(this.user)
    this.chatService.createHubConnection(this.user,id)
  }

  filterConversations(){

  }

  sendMessage(){
    let date = new Date();
    const message: MessageDto = {
      conversationId : this.currentConversation.conversationId,
      senderId : this.user['id'],
      content : this.msg,
      sentOn : date.toISOString()
    };
    this.chatService.sendMessage(message).then(() => {
      this.msg = "";
    });

  }

  ngOnDestroy(): void {
    this.chatService.closeHubConnection();
  }
}
