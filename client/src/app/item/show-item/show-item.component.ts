import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { Product } from 'src/app/_Models/Product';
import { ProductService } from 'src/app/_Services/product.service';
import { ToastrService } from 'ngx-toastr';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-show-item',
  templateUrl: './show-item.component.html',
  styleUrls: ['./show-item.component.css']
})
export class ShowItemComponent implements OnInit {

  suggested: number;
  product: Product;
  mapMarker = faMapMarkerAlt
  products: Product[] = [];
  user: number;

  constructor(private productService: ProductService, private activatedRoute: ActivatedRoute, private _location: Location, private toastR: ToastrService, private router : Router) { }

  ngOnInit(): void {
    this.productService.getProduct(Number(this.activatedRoute.snapshot.paramMap.get('id'))).subscribe(product => {
      this.product = product;
    })
    setTimeout(() => {
      this.loadProducts();
    }, 2000);
    this.user = localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user'))['id'] : undefined
  }

  goBack() {
    this._location.back();
  }

  loadProducts() {
    return this.productService.getProductOfCategory(this.product.categoryId).subscribe(products => {
      let i: number = 0;
      products.result.forEach(element => {
        if (element.productId != this.product.productId) {
          this.products.push(element)
        }
      });
      this.suggested = Math.floor(12 / products.result.length)
    });
  }

  addFavorite() {
    if (this.user != undefined) {
      this.productService.addToFavorites(this.product.productId).subscribe({
        next: (n) => this.toastR.success("Item added successfully."),
        error: (e) => this.toastR.error(e.error),
        complete: () => this.router.navigateByUrl('/favorites')
      })
    }
    else{
      this.toastR.error("Log in first");
      this.router.navigateByUrl('/login')
    }
  }

  sendMessage(){
    if(this.user != undefined){
      this.router.navigate(['messages'],{ queryParams : {product: this.product.productId}});
    }else{
      this.router.navigateByUrl('/login');
    }
  }

}
