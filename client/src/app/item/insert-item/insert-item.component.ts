import { Component, Injectable, OnInit } from '@angular/core';
import { Product } from 'src/app/_Models/Product';
import { ProductService } from '../../_Services/product.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CategoryService } from '../../_Services/category.service';
import { Category } from '../../_Models/Category';
import { UpdateProductDto } from '../../_Models/UpdateProductDto';
import { ToastrService } from 'ngx-toastr';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ProductDto } from 'src/app/_Models/ProductDto';
import { Observable } from 'rxjs';
import { Photo } from 'src/app/_Models/Photo';
import { v4 } from 'uuid';
import { ref, uploadBytes, Storage } from '@angular/fire/storage';

@Injectable({
  providedIn: 'root'
})
@Component({
  selector: 'app-insert-item',
  templateUrl: './insert-item.component.html',
  styleUrls: ['./insert-item.component.css']
})
export class InsertItemComponent implements OnInit {
  label: string = 'insert';
  product: Product;
  categories: Category[];
  productForm: FormGroup;
  imagePath: SafeResourceUrl = "../../../assets/Images/no-image.jpg";
  files: File[];
  photos$: Observable<Photo[]>;

  constructor(private productService: ProductService,
    private activatedRoute: ActivatedRoute,
    private _router: Router,
    private categoryService: CategoryService,
    private fb: FormBuilder,
    private toast: ToastrService,
    private domSanitizer: DomSanitizer,
    private storage: Storage,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.url.subscribe(response => {
      this.label = response[0].path == 'edit-item' ? 'Edit' : 'Insert';
    })
    this.initForm();
    this.loadProduct();
    this.loadCategories();

  }

  initForm() {
    this.productForm = this.fb.group({
      id: [''],
      name: ['', Validators.required],
      description: ['', Validators.required],
      categoryId: [null, Validators.required],
      price: ['', Validators.required],
      isSale: ['']
    })
  }

  loadProduct() {
    this.productService.getProduct(Number(this.activatedRoute.snapshot.paramMap.get('id'))).subscribe(response => {
      if (response == undefined)
        return;
      this.product = response;
      this.productForm.setValue({
        id: this.product.productId,
        name: this.product.name,
        description: this.product.description,
        categoryId: this.product.categoryId,
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
    if (this.productForm.status == 'INVALID') {
      this.toast.error('Invalid product!')
      return
    }
    if (this.product != undefined) {
      const updateProduct: UpdateProductDto = {
        categoryId: this.productForm.controls['categoryId'].value,
        name: this.productForm.controls['name'].value,
        description: this.productForm.controls['description'].value,
        price: this.productForm.controls['price'].value,
        isSale: this.productForm.controls['isSale'].value
      }
      this.productService.editProduct(updateProduct, this.product.productId).subscribe({
        next: (v) => {
          this.toast.success("Item edited successfully")
          this._router.navigateByUrl('/my-items')
        },
        error: (e) => {
          this.toast.error(e.error)
        }
      })
    } else {
      const user = JSON.parse(localStorage.getItem('user'));
      let date = new Date();
      let photoArray: Photo[] = [];

      Array.from(this.files).forEach(file => {
        let ext = file.type.split('/').pop()
        let photo: Photo = {
          filename: v4() + '.' + ext,
          isMain: photoArray.length == 0
        }
        photoArray.push(photo)
      });

      const newProduct: ProductDto = {
        categoryId: this.productForm.controls['categoryId'].value,
        name: this.productForm.controls['name'].value,
        description: this.productForm.controls['description'].value,
        price: this.productForm.controls['price'].value,
        isSale: this.productForm.controls['isSale'].value == '' ? false : true,
        insertedBy: user['id'],
        insertedOn: date.toISOString(),
        photos: photoArray
      }

      let photos: Photo[];

      this.productService.registerProduct(newProduct).subscribe(response => {
        if (response['productId'] != undefined || response['productId'] != null) {
          let i = 0;
          let ext = this.files[i].type.split('/').pop();
          Array.from(response['photos']).forEach(photo => {
            const storageRef = ref(this.storage, `images/${photo['filename']}`);
            const metadata = {
              contentType: 'images/' + ext
            }

            const uploadTask = uploadBytes(storageRef, this.files[0], metadata);
            uploadTask.then(() => {
              this.toast.success(`File : ${photo['filename']}, uploaded successfully`);
              this._router.navigateByUrl('/my-items')
            })
          });
        }
      })
    }
  }

  fileChanges(event: any) {
    const files: File[] = event.target.files;
    if (files.length == 0) {
      return;
    }

    Array.from(files).forEach(file => {
      if (file.type.match("image/*") == null) {
        this.toast.error("Only Images allowed")
        return;
      }
    });
    let ext = files[0].type.split('/').pop()
    console.log(ext)

    const reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onload = () => {
      this.imagePath = reader.result.toString();
    }
    this.files = files;
  }
}