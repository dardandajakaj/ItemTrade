import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PaginationModule } from 'ngx-bootstrap/pagination'

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ModalModule.forRoot(),
    FontAwesomeModule,
    PaginationModule.forRoot()
  ],
  exports:[
    ModalModule,
    FontAwesomeModule,
    PaginationModule
  ]
})
export class SharedModule { }
