import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_Services/account-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  title = 'The Course App 1';
  user: any;

  constructor(private http: HttpClient, private accountService: AccountService) {}
  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }
  setCurrentUser(){
    let localUser = localStorage.getItem('user');

    if(localUser != null){
      this.accountService.setCurrentUser(JSON.parse(localUser))
    }
  }

  getUsers(){
    this.http.get("https://localhost:7103/api/user").subscribe({
      next: (response) => this.user = response,
      error: (error) => console.log(error)
    })
  }
}
