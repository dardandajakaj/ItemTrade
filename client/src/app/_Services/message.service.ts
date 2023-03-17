import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Message } from '../_Models/Message';


@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl = environment.url;

  constructor(private http: HttpClient) { }

  getMessage(sender: string) {
    let params = new HttpParams();
    params.append('sender', sender);
    this.http.get(this.baseUrl + "message", { headers: this.loadToken(), params: params });
  }

  sendMessage(message: Message) {
    return this.http.post(this.baseUrl + 'message/send', message, { headers: this.loadToken() });
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'message/delete/' + id, { headers: this.loadToken() });
  }

  loadToken() {
    let token = JSON.parse(localStorage.getItem('user'));

    let headerOptions = new HttpHeaders().set("Authorization", "Bearer " + token['token']);
    return headerOptions;
  }
}
