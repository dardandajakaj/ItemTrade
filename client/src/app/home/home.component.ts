import { Component, OnInit } from '@angular/core';
import { Category } from '../_Models/Category';
import { CategoryService } from '../_Services/category.service';
import { ProductService } from '../_Services/product.service';
import { Product } from '../_Models/Product';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { Pagination } from '../_Models/Pagination';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  mapMarker = faMapMarkerAlt;
  public categories: Category[];
  public products: Product[];
  pagination : Pagination;
  pageNumber : number = 1;
  itemsPerPage: number = 5;
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
    this.productService.getProducts(this.pageNumber, this.itemsPerPage).subscribe(response => {
      this.products = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any){
    this.pageNumber = event.page;
    this.getProducts();
    window.scroll({
      top:0,
      left: 0
    })
  }
}
