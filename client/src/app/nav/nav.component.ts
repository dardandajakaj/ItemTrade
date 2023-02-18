import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_Services/account-service.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  item: any = {};
  model: any = {};
  constructor(public accountService: AccountService, private router: Router, private toastR: ToastrService) { }

  ngOnInit(): void {
  }

  search() {
    this.router.navigate(['/'], { queryParams: { item: this.item.name } });
  }
  onLogout() {
    this.accountService.logout();
    this.router.navigateByUrl('/login')
  }
  goHome() {
    throw new Error('Method not implemented.');
  }
}
