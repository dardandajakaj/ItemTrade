import { Component, OnInit } from '@angular/core';
import { Category } from '../_Models/Category';
import { LabelType, Options } from '@angular-slider/ngx-slider';
import { CategoryService } from '../_Services/category.service';
import { ProductService } from '../_Services/product.service';
import { Product } from '../_Models/Product';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  minValue: number = 1;
  maxValue: number = 2000;
  options: Options = {
    floor: 0,
    ceil: 2000,
    translate: (value: number, label: LabelType): string => {
      switch (label) {
        case LabelType.Low:
          return "Min price:$" + value;
        case LabelType.High:
          return "Max price:$" + value;
        default:
          return "$" + value;
      }
    }
  };
  public categories: Category[];
  public products: Product[];
  constructor(private categoryService: CategoryService, private productService: ProductService) {

  }

  ngOnInit(): void {
    this.getCategories();
    this.getProducts();
  }

  getCategories(){
    this.categoryService.getCategories().subscribe(categories => {
      this.categories = categories;
    })
  }

  getProducts(){
    this.productService.getProducts().subscribe(products => {
      this.products = products;
    })
  }

  navigateTo(categoryId: number){
    console.log(categoryId)
  }

}
