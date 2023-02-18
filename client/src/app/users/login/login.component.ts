import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faFacebookF, faGooglePlusG, faLinkedinIn } from '@fortawesome/free-brands-svg-icons';
import { AccountService } from 'src/app/_Services/account-service.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  facebook = faFacebookF;
  google = faGooglePlusG;
  linkedin = faLinkedinIn;
  active: boolean = false;
  signupForm: FormGroup;
  signinForm: FormGroup;

  constructor(private accountService: AccountService, public fb: FormBuilder, private location: Location, private toastR : ToastrService) { }

  ngOnInit(): void {
    this.FormInit();
  }

  FormInit() {

    this.signupForm = this.fb.group({
      fullname: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      street: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      phone: ['', [Validators.required, Validators.pattern('^(\\+?1)\\s?\\(?[0-9]{3}\\)?\\s?[0-9]{3}\\-?[0-9]{4}')]],
      dob: ['', Validators.required]
    });

    this.signinForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    })
  }

  toggleActive() {
    this.active = !this.active
  }

  signUp() {
    if (this.signupForm.status === 'INVALID'){
      this.toastR.error(this.signupForm.status)
      return;
    }
    this.accountService.signup(this.signupForm.value).subscribe({
      next: (v) => this.location.back(),
      error: (e) => this.toastR.error(e.error)
    })
  }

  signIn() {
    if (this.signinForm.status === "INVALID") {
      this.toastR.error(this.signinForm.status)
      return;
    }
    this.accountService.login(this.signinForm.value).subscribe({
      next: (v) => this.location.back(),
      error: (e) => this.toastR.error(e.error)
    })
  }
}
