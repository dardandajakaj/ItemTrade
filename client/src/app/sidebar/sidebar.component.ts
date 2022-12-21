import { Component, Input, OnInit } from '@angular/core';
import { Category } from '../_Models/Category';
import { LabelType, Options } from '@angular-slider/ngx-slider';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  @Input() categories: Category[];
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
  constructor() { }

  ngOnInit(): void {
  }

}
