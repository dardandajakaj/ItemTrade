import { Component, OnInit, TemplateRef} from '@angular/core';
import { UrlSerializer } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { RegisterDto } from '../_Models/RegisterDto';
import { AccountService } from '../_Services/account-service.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  modalRef?: BsModalRef;
  item: any = {};
  model: any = {};
  constructor(private modalService: BsModalService, public accountService: AccountService) {}



  ngOnInit(): void {
  }

  // getCurrentUser(){
  //   this.accountService.currentUser$.subscribe(user => {

  //   })
  // }

  search(){
    console.log(this.item)
  }
  openSigninForm(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  openLoginForm(template: TemplateRef<any>){
    this.modalRef = this.modalService.show(template);
  }

  onLogin(){
    console.log(this.model);
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log("this is: "+response);
        this.modalRef?.hide();
      },
      error: (err) => console.log(this.model)
    })

  }

  onSignUp(){
    if(this.model.password != this.model.password2){
      console.log("Password do not match!")
    }else{
      const user = {} as RegisterDto;
      user.username = this.model.username;
      user.password = this.model.password;
      this.accountService.signup(user).subscribe({
        next: (response) => {
          console.log("this is the response: " + response);

        },
        error: (err) => console.log(this.model)
      })
      this.modalRef?.hide();
    }
  }

  onLogout(){
    this.accountService.logout();

  }
}
