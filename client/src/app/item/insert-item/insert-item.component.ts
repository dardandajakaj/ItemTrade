import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/_Models/Product';
import { ProductService } from '../../_Services/product.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CategoryService } from '../../_Services/category.service';
import { Category } from '../../_Models/Category';
import { UpdateProductDto } from '../../_Models/UpdateProductDto';
import { ProductDto } from '../../_Models/ProductDto';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-insert-item',
  templateUrl: './insert-item.component.html',
  styleUrls: ['./insert-item.component.css']
})
export class InsertItemComponent implements OnInit {
  label: string = 'insert';
  product: Product;
  categories: Category[];
  productForm : FormGroup;

  constructor(private productService: ProductService, private activatedRoute: ActivatedRoute, private _router: Router, private categoryService: CategoryService, private fb: FormBuilder, private toast: ToastrService) { }

  ngOnInit(): void {
    this.activatedRoute.url.subscribe(response => {
      this.label = response[0].path == 'edit-item' ? 'Edit' : 'Insert';
    })
    this.initForm();
    this.loadProduct();
    this.loadCategories();

  }

  initForm(){
    this.productForm = this.fb.group({
      id: [''],
      name: ['', Validators.required],
      description: ['', Validators.required],
      category: [null, Validators.required],
      price: ['', Validators.required],
      isSale: ['']
    })
  }

  loadProduct() {
    this.productService.getProduct(Number(this.activatedRoute.snapshot.paramMap.get('id'))).subscribe(response => {
      if(response == undefined)
      return;
      this.product = response;
      this.productForm.setValue({
        id: this.product.productId,
        name: this.product.name,
        description: this.product.description,
        category: this.product.categoryId,
        price: this.product.price,
        isSale: this.product.isSale
      })
    })
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe(response => {
      this.categories = response;
    })
  }

  checkInput(event) {
    const rule = new RegExp('[0-9]|[.]|[,]')
    if (!rule.test(event.key) && event.key != 'Backspace' && event.key != 'Tab' && event.key != 'Control') {
      event.preventDefault();
    }
  }

  saveItem() {
    if (this.product != undefined) {
      const updateProduct: UpdateProductDto = {
        categoryId: this.productForm.controls['category'].value,
        name: this.productForm.controls['name'].value,
        description: this.productForm.controls['description'].value,
        price: this.productForm.controls['price'].value,
        isSale: this.productForm.controls['isSale'].value
      }
      this.productService.editProduct(updateProduct, this.product.productId).subscribe({
        next: (v) => {
          this.toast.success("Item edited successfully")
          this._router.navigateByUrl('/my-items')},
        error: (e) => this.toast.error(e)
      })
    } else {
      const user = JSON.parse(localStorage.getItem('user'));
      let date = new Date();
      const productDto : ProductDto = {
        name: this.productForm.controls['name'].value,
        description: this.productForm.controls['description'].value,
        categoryId: this.productForm.controls['category'].value,
        price: this.productForm.controls['price'].value,
        isSale: this.productForm.controls['isSale'].value ? true: false,
        insertedBy: user['id'],
        insertedOn: date.toISOString()
      }

      this.productService.registerProduct(productDto).subscribe({
        next: (v) => {
          this.toast.success("Successfully Saved")
        },
        error: (e) => {
          this.toast.error(e)
        },
        complete: ()=>{
          this._router.navigate(['/my-items'])
        }
      })
    }
  }
}
