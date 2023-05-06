import { Component, OnDestroy, OnInit } from '@angular/core';
import { Conversation } from '../_Models/Conversation';
import { ChatService } from '../_Services/chat.service';
import { ToastrService } from 'ngx-toastr';
import { Observable, filter, map } from 'rxjs';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { MessageDto } from '../_Models/MessageDto';
import { Product } from '../_Models/Product';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../_Services/product.service';
import { ConversationDto } from '../_Models/ConversationDto';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.css']
})
export class ConversationComponent implements OnInit, OnDestroy {
  product: Product;
  conversations$ = new Observable<Conversation[]>;
  currentConversation: Conversation;
  user = JSON.parse(localStorage.getItem('user'));
  faPaperPlane = faPaperPlane;
  chatter: string = "";
  msg: string;

  constructor(
    public chatService: ChatService,
    private route: ActivatedRoute,
    private productService: ProductService,
    private router: Router,
    private toastR: ToastrService) { }

  ngOnInit(): void {
    this.getAllConversations();
    this.route.queryParams.subscribe(params => {
      if (params["product"] !== undefined) {
        this.conversations$.subscribe(
          data =>
            this.currentConversation = data.find(x => x.productId == params["product"] && (x.receiverId == this.user.id || x.senderId == this.user.id)))
        if (this.currentConversation == undefined || this.currentConversation == null) {
          this.productService.getProduct(params["product"]).subscribe(response => {
            this.product = response
            const conversation: ConversationDto = {
              productId: this.product.productId,
              senderId: this.user.id,
              receiverId: this.product.insertedBy
            }
            this.chatService.createConversation(conversation).subscribe(data => {
              this.router.navigateByUrl(window.location.pathname);
            })
          })
        }
      }
    })
  }

  getProductConversations(productId: number) {
    this.conversations$ = this.chatService.getProductConversations(productId);
  }

  getAllConversations() {
    this.conversations$ = this.chatService.getUserConversations(this.user['id']);
  }
  openConversation(conversationId: number) {
    this.toastR.info("Navigate to " + conversationId);
  }

  selectChat(id: number) {
    this.msg = "";
    this.conversations$.subscribe(data => this.currentConversation = data.find(x => x.conversationId == id))
    this.chatService.createHubConnection(this.user, id)
  }

  filterConversations() {
    if (this.chatter != "") {
      this.conversations$ = this.conversations$.pipe(
        map(
          conversations => conversations.filter(
            conversation =>
              conversation.receiver.toLowerCase().includes(this.chatter.toLowerCase())
              || conversation.sender.toLowerCase().includes(this.chatter.toLowerCase())
              || conversation.product.toLowerCase().includes(this.chatter.toLowerCase())
          )))
    }else{
      this.conversations$ = this.conversations$.pipe(
        map(
          conversations => conversations.filter(() => true)
        )
      )
    }


  }

  sendMessage() {
    let date = new Date();
    const message: MessageDto = {
      conversationId: this.currentConversation.conversationId,
      senderId: this.user['id'],
      content: this.msg,
      sentOn: date.toISOString()
    };
    this.chatService.sendMessage(message).then(() => {
      this.msg = "";
    });

  }

  ngOnDestroy(): void {
    this.chatService.closeHubConnection();
  }
}
