import {Routes} from '@angular/router';
import {BaseLayoutComponent} from "./modules/shared/layouts/base-layout/base-layout.component";
import {PageNotFoundComponent} from "./modules/shared/page-not-found/page-not-found.component";

export const routes: Routes = [
  {path: 'not-found', pathMatch: "full", component: PageNotFoundComponent},
  {path: '', pathMatch: 'full', component: BaseLayoutComponent},
  {path: '**', component: PageNotFoundComponent},
];
