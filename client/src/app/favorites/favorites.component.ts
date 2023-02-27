import { Component, OnInit } from '@angular/core';
import { Category } from '../_Models/Category';
import { Observable } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { Product } from '../_Models/Product';
import { environment } from 'src/environments/environment';
import { Pagination } from '../_Models/Pagination';
import { Filter } from '../_Models/Filter';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { CategoryService } from '../_Services/category.service';
import { ProductService } from '../_Services/product.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-favorites',
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.css']
})
export class FavoritesComponent implements OnInit {

  mapMarker = faMapMarkerAlt;
  public categories$: Observable<Category[]>;
  public products: Product[];
  pagination: Pagination;
  pageNumber: number = 1;
  item: string;
  itemsPerPage: number = environment.itemsPerPage;
  filters: Filter = {
    name: '-1',
    category: 0,
    minPrice: 0,
    maxPrice: 0,
    sort: 0
  }
  constructor(private categoryService: CategoryService, private productService: ProductService, private router: Router, private toastR: ToastrService) { }

  ngOnInit(): void {
    this.getCategories();
    this.getProducts();
  }

  getCategories() {
    this.categories$ = this.categoryService.getCategories();
  }

  getProducts() {
    this.productService.getFilteredProducts(this.filters, true).subscribe((response) => {
      this.products = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.getProducts();
    window.scroll({
      top: 0,
      left: 0
    })
  }

  filterChanged(filter: FormGroup) {
    this.checkFilter(filter)
    this.productService.getFilteredProducts(this.filters, true, this.pageNumber, environment.itemsPerPage).subscribe(response => {
      this.products = response.result;
      this.pagination = response.pagination;
    })
  }

  changeItemSize() {
    environment.itemsPerPage = this.itemsPerPage
    setTimeout(() => {
      this.ngOnInit()
    }, 1000);
  }

  checkFilter(filter: FormGroup) {
    this.filters.name = (filter['name'] == '' || filter['name'] == null) ? "-1" : filter['name'];
    this.filters.category = (filter['category'] == '' || filter['category'] == null) ? 0 : filter['category'];
    this.filters.minPrice = (filter['minPrice'] == '' || filter['minPrice'] == null) ? 0 : filter['minPrice'];
    this.filters.maxPrice = (filter['maxPrice'] == '' || filter['maxPrice'] == null) ? 0 : filter['maxPrice'];
    this.filters.sort = (filter['sort'] == '' || filter['sort'] == null) ? 0 : filter['sort'];
  }

  removeFavorite(productId : number){
    this.productService.removeFavorite(productId).subscribe({
      next: (v) => this.toastR.success("Successfully deleted!"),
      error: (e) => this.toastR.error("Something went south!"),
      complete: ()=> this.ngOnInit()
    })
  }
}
