import {Routes} from '@angular/router';
import {BaseLayoutComponent} from "./modules/shared/layouts/base-layout/base-layout.component";
import {PageNotFoundComponent} from "./modules/shared/components/page-not-found/page-not-found.component";
import {LoginComponent} from "./modules/login/login.component";

export const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {
    path: 'admin', component: BaseLayoutComponent,
    loadChildren: () => import('./modules/librarian/librarian.module').then((m) => m.LibrarianModule),
  },
  {
    path: '', component: BaseLayoutComponent,
    loadChildren: () => import('./modules/member/member.module').then((m) => m.MemberModule),
  },
  {path: 'not-found', pathMatch: "full", component: PageNotFoundComponent},
  {path: '**', component: PageNotFoundComponent},
];
