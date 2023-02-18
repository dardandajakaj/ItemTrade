import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundErrorComponent } from './errors/not-found-error/not-found-error.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { HomeComponent } from './home/home.component';
import { ShowItemComponent } from './item/show-item/show-item.component';
import { CreateProfileComponent } from './users/create-profile/create-profile.component';
import { EditProfileComponent } from './users/edit-profile/edit-profile.component';
import { LoginComponent } from './users/login/login.component';
import { MyItemsComponent } from './item/my-items/my-items.component';
import { InsertItemComponent } from './item/insert-item/insert-item.component';
import { AuthGuard } from './_guards/auth.guard';


const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '', runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'my-items', component: MyItemsComponent },
      { path: 'edit-item/:id', component: InsertItemComponent },
      { path: 'insert-item', component: InsertItemComponent },
    ]
  },
  { path: 'category/:id', component: HomeComponent },
  { path: 'create-profile', component: CreateProfileComponent },
  { path: 'edit-profile', component: EditProfileComponent },
  { path: 'item/:id', component: ShowItemComponent },

  { path: 'not-found', component: NotFoundErrorComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', component: NotFoundErrorComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
