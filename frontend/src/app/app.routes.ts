import {Routes} from '@angular/router';
import {BaseLayoutComponent} from './modules/shared/layouts/base-layout/base-layout.component';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule),
  },
  {
    path: 'auth',
    loadChildren: () => import('./modules/base/users/users.module').then(m => m.UsersModule),
  },
  {
    path: '',
    component: BaseLayoutComponent,
    loadChildren: () => import('./modules/base/base.module').then(m => m.BaseModule)
  },
];
