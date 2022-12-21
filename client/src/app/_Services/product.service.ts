import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Product } from '../_Models/Product';
import { PaginatedResult } from '../_Models/Pagination';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  paginatedResult: PaginatedResult<Product[]> = new PaginatedResult<Product[]>();

  constructor(private http: HttpClient) { }

  getProducts(page?: number, itemsPerPage?: number){
    let params = new HttpParams();
    if(page != null && itemsPerPage != null){
      params = params.append('pageNumber', page.toString());
      params = params.append('itemsPerPage', itemsPerPage.toString());
    }
    return this.http.get<Product[]>(environment.url + "product/items", {observe: 'response', params}).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if(response.headers.get("Pagination") !== null){
          this.paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"))
        }
        return this.paginatedResult;
      })
    )
  }

  getProduct(id: number){
    return this.http.get<Product>(environment.url + "product/item/" + id);
  }

  getProductOfCategory(id :number){
    return this.http.get<Product[]>(environment.url + "product/items/category/" + id);
  }
}
