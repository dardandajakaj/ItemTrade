import { Component, OnInit } from '@angular/core';
import { Category } from '../_Models/Category';
import { CategoryService } from '../_Services/category.service';
import { ProductService } from '../_Services/product.service';
import { Product } from '../_Models/Product';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { Pagination } from '../_Models/Pagination';
import { environment } from '../../environments/environment';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  mapMarker = faMapMarkerAlt;
  public categories: Category[];
  public products: Product[];
  pagination: Pagination;
  pageNumber: number = 1;
  itemsPerPage: number = environment.itemsPerPage;
  constructor(private categoryService: CategoryService, private productService: ProductService, public router: ActivatedRoute) { }

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
    let id: number;
    let path: string;

    this.router.url.subscribe(url => {
      if (url.length > 0) {
        id = Number.parseInt(url[1].path);
        path = url[0].path;
      }
    })

    if (id != undefined && path != undefined) {
      this.productService.getProductOfCategory(id, this.pageNumber, environment.itemsPerPage).subscribe(response => {
        this.products = response.result;
        this.pagination = response.pagination;
      })
    } else {
      this.productService.getProducts(this.pageNumber, environment.itemsPerPage).subscribe(response => {
        this.products = response.result;
        this.pagination = response.pagination;
      })
    }
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.getProducts();
    window.scroll({
      top: 0,
      left: 0
    })
  }

  changeItemSize() {
    environment.itemsPerPage = this.itemsPerPage
    setTimeout(() => {
      this.ngOnInit()
    }, 1000);
  }
}
