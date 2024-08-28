import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {SearchPageComponent} from "./pages/search-page/search-page.component";
import {BookItemComponent} from "./pages/book-item/book-item.component";

const routes: Routes = [
  {
    path: "search",
    component: SearchPageComponent,
  },
  {
    path: "detail/:id",
    component: BookItemComponent,
  },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BooksModule {
}
