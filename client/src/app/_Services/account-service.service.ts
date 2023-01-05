import { HttpClient } from '@angular/common/http';
import { Injectable, Output } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_Models/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.url;
  constructor(private http: HttpClient) { }
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  login(model:any){
    return this.http.post<User>(this.baseUrl + "account/login", model).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }

  signup(user: any){
    return this.http.post<User>(this.baseUrl + "account/register", user).pipe(
      map((response: User) =>{
        const userResponse = response;
        if(user){
          localStorage.setItem('user',JSON.stringify(userResponse));
          this.currentUserSource.next(userResponse);
        }else{
          console.log("Error!!!")
        }
      })
    )
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
