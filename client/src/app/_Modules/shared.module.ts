import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination'

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ModalModule.forRoot(),
    NgxSliderModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    PaginationModule.forRoot()
  ],
  exports:[
    ModalModule,
    NgxSliderModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    PaginationModule
  ]
})
export class SharedModule { }
