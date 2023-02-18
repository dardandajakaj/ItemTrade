import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Category } from '../_Models/Category';
import { LabelType, Options } from '@angular-slider/ngx-slider';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  @Input() categories: Category[];
  @Output() searchEventEmitter = new EventEmitter<FormGroup>();
  // @Output() addPriceRangeEvent = new EventEmitter<number[]>();
  // @Output() addPriceFilterEvent = new EventEmitter<boolean>();
  // @Output() addDateFilterEvent = new EventEmitter<boolean>();
  sideForm : FormGroup;
  minValue : number = 1;
  maxValue : number = 2000;
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
  constructor(public fb : FormBuilder) { }

  ngOnInit(): void {
    this.initForm();
  }

  initForm(){
    this.sideForm = this.fb.group({
      name : '',
      category: '',
      minPrice: '',
      maxPrice: '',
      sort: ''
    })
  }

  searchEvent(){
    this.searchEventEmitter.emit(this.sideForm.value)
  }

  resetFilter(){
    this.sideForm.reset();
    this.searchEventEmitter.emit(this.sideForm.value)
  }

  // priceRangeEvent(){
  //   this.addPriceRangeEvent.emit([this.minValue, this.maxValue]);
  // }
}
