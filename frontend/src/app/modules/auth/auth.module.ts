import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from './login/login.component';
import {RegisterComponent} from './register/register.component';
import { AuthService } from '../../services/auth/auth.service';

const routes: Routes = [
  {path: 'login', title: 'Đăng nhập', component: LoginComponent},
  {path: 'register', title: 'Đăng ký', component: RegisterComponent},
];

@NgModule({
  declarations: [],
  providers: [{ provide: 'AUTH_SERVICE_INJECTOR', useClass: AuthService }],
  // providers: [AuthService],

  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthModule {
}
