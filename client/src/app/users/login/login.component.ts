import { Component, OnInit } from '@angular/core';
import { faFacebookF, faGooglePlusG, faLinkedinIn } from '@fortawesome/free-brands-svg-icons';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  facebook = faFacebookF;
  google = faGooglePlusG;
  linkedin = faLinkedinIn;
  active : boolean = false;
  constructor() { }

  ngOnInit(): void {
  }

  toggleActive(){
    this.active =!this.active
    console.log(this.active)
  }

  signIn(){
    console.log("Signing in...")
  }

  signUp(){
    console.log("Signing up...")
  }
}
