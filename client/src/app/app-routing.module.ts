import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundErrorComponent } from './errors/not-found-error/not-found-error.component';
import { HomeComponent } from './home/home.component';
import { ProductComponent } from './product/product.component';
import { ProfileComponent } from './profile/profile.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'not-found', component: NotFoundErrorComponent},
  {path: 'pruduct/:id', component: ProductComponent},
  {path: 'profile', component:ProfileComponent},
  {path: '**', component: NotFoundErrorComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
