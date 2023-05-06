import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Conversation } from '../_Models/Conversation';
import { environment } from '../../environments/environment';
import { Helpers } from '../_Helpers/Helpers';
import { Message } from '../_Models/Message';
import { MessageDto } from '../_Models/MessageDto';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { User } from '../_Models/User';
import { ConversationDto } from '../_Models/ConversationDto';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private hubConnection: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();
  constructor(public http: HttpClient) { }

  createHubConnection(user: User,conversationId: number){
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hub + "?conversationId=" + conversationId,{accessTokenFactory: () => user.token })
      .withAutomaticReconnect()
      .build();
      this.hubConnection.start().catch(error => console.log(error));

      this.hubConnection.on("MessageReceived", messages => {
        this.messageThreadSource.next(messages);
      })

      this.hubConnection.on("NewMessage", message => {
        this.messageThread$.pipe(take(1)).subscribe(messages =>{
          this.messageThreadSource.next([...messages, message]);
        })
      })
  }

  closeHubConnection(){
    if(this.hubConnection)
      this.hubConnection.stop();
  }

  header = Helpers.loadToken();

  getProductConversations(productId: number) {
    return this.http.get<Conversation[]>(environment.url + 'message/conversations/' + productId, {headers: this.header});
  }

  getUserConversations(userId: number){
    return this.http.get<Conversation[]>(environment.url + "message/conversations/user/"+ userId, {headers: this.header});
  }

  getMessages(conversationId: number){
    return this.http.get<Message[]>(environment.url + "message/" + conversationId, {headers: this.header});
  }

  async sendMessage(message: MessageDto){
    return this.hubConnection.invoke("SendMessage", message);
  }

  createConversation(conversation: ConversationDto){
    return this.http.post(environment.url + "message/conversation/create", conversation, {headers: this.header});
  }

  deleteMessage(messageId: number){
    return this.http.delete(environment.url + "message/" + messageId, {headers: this.header});
  }
}
