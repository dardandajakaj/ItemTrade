import { Component, OnInit } from '@angular/core';
import { from } from 'rxjs';
import { Category } from '../_Models/Category';
import { LabelType, Options } from '@angular-slider/ngx-slider';
import { CategoryService } from '../_Services/category.service';

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
