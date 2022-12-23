import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NotFoundErrorComponent } from './errors/not-found-error/not-found-error.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { InsertItemComponent } from './item/insert-item/insert-item.component';
import { EditItemComponent } from './item/edit-item/edit-item.component';
import { ListItemsComponent } from './item/list-items/list-items.component';
import { CreateProfileComponent } from './users/create-profile/create-profile.component';
import { EditProfileComponent } from './users/edit-profile/edit-profile.component';
import { ShowItemComponent } from './item/show-item/show-item.component';
import { CategoryComponent } from './category/category.component';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { ItemCardComponent } from './item/item-card/item-card.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { SharedModule } from './_Modules/shared.module';
import { LoginComponent } from './users/login/login.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    NotFoundErrorComponent,
    ServerErrorComponent,
    InsertItemComponent,
    EditItemComponent,
    ListItemsComponent,
    CreateProfileComponent,
    EditProfileComponent,
    ShowItemComponent,
    CategoryComponent,
    ItemCardComponent,
    SidebarComponent,
    HomeComponent,
    LoginComponent,
    TextInputComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    SharedModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
