import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {HomePageComponent} from "../shared/view-books/pages/home-page/home-page.component";
import {SearchPageComponent} from "../shared/view-books/pages/search-page/search-page.component";

export const routes: Routes = [
  {
    path: 'search',
    component: SearchPageComponent,
  },
  {
    path: '',
    component: HomePageComponent,
  }
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule, RouterModule.forChild(routes)
  ],
  exports: [RouterModule],
})
export class MemberModule {
}
