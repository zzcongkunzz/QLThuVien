import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {SearchPageComponent} from "./pages/search-page/search-page.component";
import {BookItemComponent} from "./pages/book-item/book-item.component";
import { AddBookPageComponent } from './pages/add-book-page/add-book-page.component';
import { CategoryService } from '../../../services/category/category.service';
import { EditBookPageComponent } from './pages/edit-book-page/edit-book-page.component';

const routes: Routes = [
  {
    path: "search",
    component: SearchPageComponent,
  },
  {
    path: "add",
    component: AddBookPageComponent,
  },
  {
    path: "edit/:id"
  , component: EditBookPageComponent,
  },
  {
    path: "detail/:id",
    component: BookItemComponent,
  },
];

@NgModule({
  declarations: [],
  providers: [ CategoryService ],
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BooksModule {
}
