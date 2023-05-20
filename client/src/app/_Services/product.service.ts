import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Product } from '../_Models/Product';
import { PaginatedResult } from '../_Models/Pagination';
import { map } from 'rxjs';
import { ProductDto } from '../_Models/ProductDto';
import { UpdateProductDto } from '../_Models/UpdateProductDto';
import { Filter } from '../_Models/Filter';
import { Helpers } from '../_Helpers/Helpers';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  paginatedResult: PaginatedResult<Product[]> = new PaginatedResult<Product[]>();

  constructor(private http: HttpClient) {

  }

  header = Helpers.loadToken();

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

  getFilteredProducts(filters: Filter, favorite?: boolean, page?: number, itemsPerPage?: number) {

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
    if (!favorite) {
      return this.http.get<Product[]>(environment.url + "product/items", { observe: 'response', params }).pipe(
        map(response => {
          this.paginatedResult.result = response.body;
          if (response.headers.get("Pagination") !== null) {
            this.paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"))
          }
          return this.paginatedResult;
        })
      )
    } else {
      return this.http.get<Product[]>(environment.url + 'product/favorites',{ observe: 'response', params, headers: this.header }).pipe(
        map(response => {
          this.paginatedResult.result = response.body;
          if (response.headers.get("Pagination") !== null) {
            this.paginatedResult.pagination = JSON.parse(response.headers.get("Pagination"))
          }
          return this.paginatedResult;
        })
      )
    }

  }

  getProductOfUser(id: number, page?: number, itemsPerPage?: number) {

    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('itemsPerPage', itemsPerPage.toString())
    }
    return this.http.get<Product[]>(environment.url + 'product/myitems/' + id, { headers: this.header, observe: 'response', params }).pipe(
      map(response => {
        this.paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'))
        }
        return this.paginatedResult;
      })
    );
  }

  registerProduct(product: ProductDto) {

    return this.http.post(environment.url + 'product/item/add',product, { headers: this.header });
  }

  editProduct(productDto: UpdateProductDto, id: number) {

    return this.http.put(environment.url + 'product/edit/' + id, productDto, { headers: this.header });
  }

  deleteProduct(productId: number) {

    return this.http.delete(environment.url + 'product/delete/' + productId, { headers: this.header });
  }

  addToFavorites(productId: number) {
    return this.http.post(environment.url + 'product/favorites/add', productId, { headers: this.header });
  }

  removeFavorite(productId: number) {
    return this.http.delete(environment.url + 'product/favorites/remove/' + productId, {headers: this.header});
  }
}
