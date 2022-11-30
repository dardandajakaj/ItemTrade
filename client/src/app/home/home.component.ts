import { Component, NgModule, OnInit } from '@angular/core';
import { Category } from '../_Models/Category';
import { CategoryService } from '../_Services/category.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  public categories: Category[];
  constructor(private categoryService: CategoryService) {

  }

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(){
    this.categoryService.getCategories().subscribe(categories => {
      this.categories = categories;
    })
  }

  navigateTo(categoryId: number){
    console.log(categoryId)
  }

}
