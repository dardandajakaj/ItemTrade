import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable, map } from 'rxjs';
import { AccountService } from '../_Services/account-service.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(public accountService: AccountService, private toastr: ToastrService, private router : Router) { }
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if(user != undefined){
          return true;
        }
        this.toastr.error("You shall not pass");
        this.router.navigateByUrl("/not-found");
        return false;
      })
    );
  }
}
