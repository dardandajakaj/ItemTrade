import { Component, Input, OnInit } from '@angular/core';
import { Message } from '../_Models/Message';
import { Observable } from 'rxjs';
import { Product } from '../_Models/Product';
import { AccountService } from '../_Services/account-service.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {
  @Input() product: Product;
  public messages$: Observable<Message[]>;

  user = JSON.parse(localStorage.getItem("user"));
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  loadMessages() {
    let user = localStorage.getItem('user');
    var participant = user['username'] == this.product.owner? 2:2;
  }

}
