import { Component, OnInit } from '@angular/core';
import { Product } from '../../_Models/Product';
import { Pagination } from '../../_Models/Pagination';
import { ProductService } from '../../_Services/product.service';
import { AccountService } from '../../_Services/account-service.service';
import { User } from '../../_Models/User';
import { take } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-my-items',
  templateUrl: './my-items.component.html',
  styleUrls: ['./my-items.component.css']
})
export class MyItemsComponent implements OnInit {
  products: Product[];
  pagination: Pagination;
  pageNumber: number = 1;
  itemsPerPage: number = 5;
  user: User;

  constructor(private productService: ProductService, public accountService: AccountService, private toastR: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(response => {
      this.user = response
    })
  }

  ngOnInit(): void {
    this.loadMyProducts()
  }

  loadMyProducts() {
    this.productService.getProductOfUser(this.user.id).subscribe(response => {
      this.products = response.result;
      this.pagination = response.pagination;
    })
  }

  deleteProduct(productId: number) {
    this.productService.deleteProduct(productId).subscribe({
      next: (v) => this.toastR.success("Successfully deleted!"),
      error: (e) => this.toastR.error("Something went south!"),
      complete: ()=> this.ngOnInit()
    })
  }
}
