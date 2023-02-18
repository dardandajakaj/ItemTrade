import { Location } from '@angular/common';
import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { Product } from 'src/app/_Models/Product';
import { ProductService } from 'src/app/_Services/product.service';

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

  constructor(private productService: ProductService, private activatedRoute: ActivatedRoute, private _location: Location, private router: Router) { }

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
}
