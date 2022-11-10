import { Component, OnInit } from '@angular/core';
import { Category } from '../_Models/Category';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  category: Category
  constructor() { }

  ngOnInit(): void {
  }

}
