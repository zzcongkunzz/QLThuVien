import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {HomePageComponent} from "./home/pages/home-page/home-page.component";
import {PageNotFoundComponent} from "../shared/components/page-not-found/page-not-found.component";

const routes: Routes = [
  {
    path: 'books',
    loadChildren: () =>
      import('./books/books.module').then((m) => m.BooksModule),
  },
  {
    path: 'users',
    loadChildren: () =>
      import('./users/users.module').then((m) => m.UsersModule),
  },
  {
    path: 'recommender',
    loadChildren: () =>
      import('./recommender/recommender.module').then((m) => m.RecommenderModule),
  },
  {
    path: '',
    pathMatch: 'full',
    component: HomePageComponent,
  },
  {path: 'not-found', pathMatch: 'full', component: PageNotFoundComponent},
  {path: '**', component: PageNotFoundComponent},
]

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BaseModule {
}
