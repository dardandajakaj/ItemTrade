import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundErrorComponent } from './errors/not-found-error/not-found-error.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { HomeComponent } from './home/home.component';
import { EditItemComponent } from './item/edit-item/edit-item.component';
import { ListItemsComponent } from './item/list-items/list-items.component';
import { ShowItemComponent } from './item/show-item/show-item.component';
import { CreateProfileComponent } from './users/create-profile/create-profile.component';
import { EditProfileComponent } from './users/edit-profile/edit-profile.component';
import { LoginComponent } from './users/login/login.component';


const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'create-profile', component: CreateProfileComponent},
  {path: 'edit-profile', component: EditProfileComponent},
  {path: 'items', component: ListItemsComponent},
  {path: 'item/:id', component: ShowItemComponent},
  {path: 'edit-item/:id', component: EditItemComponent},
  {path: 'not-found', component: NotFoundErrorComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'login', component: LoginComponent},
  {path: '**', component: NotFoundErrorComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
