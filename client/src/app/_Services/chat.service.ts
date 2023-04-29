import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Conversation } from '../_Models/Conversation';
import { environment } from '../../environments/environment';
import { Helpers } from '../_Helpers/Helpers';
import { Message } from '../_Models/Message';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(public http: HttpClient) { }

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

  sendMessage(message: Message){
    return this.http.post(environment.url + "/message/send", message, {headers: this.header});
  }

  createConversation(conversation: Conversation){
    return this.http.post(environment.url + "/message/conversation/create", conversation, {headers: this.header});
  }

  deleteMessage(messageId: number){
    return this.http.delete(environment.url + "message/" + messageId, {headers: this.header});
  }
}
