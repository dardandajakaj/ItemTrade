import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faMapMarkerAlt } from '@fortawesome/free-solid-svg-icons';
import { Product } from 'src/app/_Models/Product';
import { Storage, getDownloadURL, ref } from '@angular/fire/storage';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrls: ['./item-card.component.css']
})
export class ItemCardComponent implements OnInit {
  @Input() product : Product;
  mapMarker = faMapMarkerAlt;
  imagePath: SafeResourceUrl = "../../../assets/Images/no-image.jpg"
  constructor(private router : Router, private storage: Storage, private domSanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.downloadImage(this.product);
  }

  navigateTo(id : number){
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = "reload";
    this.router.navigate(["item", id]);
  }

  downloadImage(product: Product){
    if(product.photos.length == 0 || product.photos == undefined || product.photos == null){
      return;
    }
    const storageRef = this.storage;
    getDownloadURL(ref(storageRef, `images/${product.photos[0].filename}`)).then((url) => {
      this.imagePath = this.domSanitizer.bypassSecurityTrustUrl(url);
    })
  }
}
