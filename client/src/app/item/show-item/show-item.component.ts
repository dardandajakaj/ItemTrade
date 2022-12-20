import { Location } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { Product } from 'src/app/_Models/Product';
import { ProductService } from 'src/app/_Services/product.service';

@Component({
  selector: 'app-show-item',
  templateUrl: './show-item.component.html',
  styleUrls: ['./show-item.component.css']
})
export class ShowItemComponent implements OnInit {
  suggested : number;
  product: Product;
  mapMarker = faMapMarkerAlt
  products: Product[] =[];

  constructor(private productService: ProductService, private activatedRoute: ActivatedRoute, private _location: Location) { }

  ngOnInit(): void {
    this.productService.getProduct(Number(this.activatedRoute.snapshot.paramMap.get('id'))).subscribe(product => {
      this.product = product;
    })
    setTimeout(() => {
      this.loadProducts();
    }, 2000);

  }

  goBack() {
    this._location.back();
  }

  goToCategory(id: number) {
    console.log(id)
  }

  loadProduct(id: number) {
    // this.productService.getProduct(id).subscribe(product =>
    //   {
    //     this.product = product;
    //   })
    // this.loadProducts();

  }

  loadProducts() {
    return this.productService.getProductOfCategory(this.product.categoryId).subscribe(products => {
      let i: number = 0;
      products.forEach(element => {
        if(element.productId != this.product.productId){
          this.products.push(element)
        }
      });
      this.suggested = Math.floor(12/products.length)
    });
  }



}
