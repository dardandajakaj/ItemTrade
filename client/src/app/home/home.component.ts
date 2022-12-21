import { Component, OnInit } from '@angular/core';
import { Category } from '../_Models/Category';
import { CategoryService } from '../_Services/category.service';
import { ProductService } from '../_Services/product.service';
import { Product } from '../_Models/Product';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  mapMarker = faMapMarkerAlt;

  public categories: Category[];
  public products: Product[];
  constructor(private categoryService: CategoryService, private productService: ProductService) { }

  ngOnInit(): void {
    this.getCategories();
    this.getProducts();
  }

  getCategories() {
    this.categoryService.getCategories().subscribe(categories => {
      this.categories = categories;
    })
  }

  getProducts() {
    this.productService.getProducts().subscribe(products => {
      this.products = products;
    })
  }

  filter(){

  }
}
