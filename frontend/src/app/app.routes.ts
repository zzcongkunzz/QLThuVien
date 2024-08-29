import {Routes} from '@angular/router';
import {BaseLayoutComponent} from './modules/shared/layouts/base-layout/base-layout.component';

export const routes: Routes = [
  {
    // for no-users
    path: 'auth',
    loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule),
  },
  {
    // for logged-in user
    path: '',
    component: BaseLayoutComponent,
    title: 'Thư viện số',
    loadChildren: () => import('./modules/base/base.module').then(m => m.BaseModule)
  },
];
