import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Product } from '../_Models/Product';
import { PaginatedResult } from '../_Models/Pagination';
import { map } from 'rxjs';
import { ProductDto } from '../_Models/ProductDto';
import { UpdateProductDto } from '../_Models/UpdateProductDto';
import { Filter } from '../_Models/Filter';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  paginatedResult: PaginatedResult<Product[]> = new PaginatedResult<Product[]>();

  constructor(private http: HttpClient) {

  }

  getProducts(page?: number, itemsPerPage?: number) {
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('itemsPerPage', itemsPerPage.toString());
    }
    return this.http.get<Product[]>(environment.url + "product/items", { observe: 'response', params }).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if (response.headers.get("Pagination") !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"))
        }
        return this.paginatedResult;
      })
    )
  }

  getProduct(id: number) {
    return this.http.get<Product>(environment.url + "product/item/" + id);
  }

  getProductOfCategory(id: number, page?: number, itemsPerPage?: number) {
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('itemsPerPage', itemsPerPage.toString());
    }
    return this.http.get<Product[]>(environment.url + "product/items/category/" + id, { observe: 'response', params }).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if (response.headers.get("Pagination") !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"))
        }
        return this.paginatedResult;
      })
    );
  }

  getFilteredProducts(filters: Filter, page?: number, itemsPerPage?: number) {

    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('itemsPerPage', itemsPerPage.toString());
    }
    params = params.append('name', filters.name)
    params = params.append('category', filters.category);
    params = params.append('minPrice', filters.minPrice);
    params = params.append('maxPrice', filters.maxPrice);
    params = params.append('sort', filters.sort);
    return this.http.get<Product[]>(environment.url + "product/items", { observe: 'response', params }).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if (response.headers.get("Pagination") !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"))
        }
        return this.paginatedResult;
      })
    )
  }

  getProductOfUser(id: number, page?: number, itemsPerPage?: number) {

    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('itemsPerPage', itemsPerPage.toString())
    }
    return this.http.get<Product[]>(environment.url + 'product/myitems/' + id, { headers: this.loadToken(), observe: 'response', params }).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'))
        }
        return this.paginatedResult;
      })
    );
  }

  registerProduct(productDto: ProductDto) {

    return this.http.post(environment.url + 'product/item/add', productDto, { headers: this.loadToken() });
  }

  editProduct(productDto: UpdateProductDto, id: number) {

    return this.http.put(environment.url + 'product/edit/' + id, productDto, { headers: this.loadToken() });
  }

  deleteProduct(productId: number) {

    return this.http.delete(environment.url + 'product/delete/' + productId, { headers: this.loadToken() });
  }

  loadToken() {
    let token = JSON.parse(localStorage.getItem('user'));

    let headerOptions = new HttpHeaders().set("Authorization", "Bearer " + token['token']);
    return headerOptions;
  }

}
