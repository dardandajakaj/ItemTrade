import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { Product } from 'src/app/_Models/Product';

@Component({
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrls: ['./item-card.component.css']
})
export class ItemCardComponent implements OnInit {
  @Input() product : Product;
  mapMarker = faMapMarkerAlt
  constructor(private router : Router) { }

  ngOnInit(): void {
  }

  navigateTo(id : number){
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = "reload";
    this.router.navigate(["item", id]);
  }
}
