import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  
  title = 'The Course App 1';
  users: any;

  constructor(private http: HttpClient) {}
  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(){
    // this.http.get("https://localhost:7103/api/user").subscribe(
    //   response => {
    //     this.users = response;
    //   },
    //   error => {
    //     console.log(error);
    //   });
    this.http.get("https://localhost:7103/api/user").subscribe({
      next: (response) => this.users = response,
      error: (error) => console.log(error)
    })
  }
}
