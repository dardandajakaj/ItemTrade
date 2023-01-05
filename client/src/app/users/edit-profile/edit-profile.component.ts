import { Component, OnInit } from '@angular/core';
import { Product } from '../../_Models/Product';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  product: Product;

  constructor() { }

  ngOnInit(): void {
  }

}
