import {Component, Inject} from '@angular/core';
import {HeaderComponent} from '../../components/header/header.component';
import {RouterModule, RouterOutlet} from '@angular/router';
import {CommonModule} from '@angular/common';
import { AuthService } from '../../../../services/auth/auth.service';
import { AuthModule } from '../../../auth/auth.module';

@Component({
  selector: 'app-base-layout',
  standalone: true,
  // providers: [{ provide: 'AUTH_SERVICE_INJECTOR', useClass: AuthService }],
  imports: [HeaderComponent, RouterOutlet, RouterModule, CommonModule, AuthModule],
  templateUrl: './base-layout.component.html',
  styleUrl: './base-layout.component.scss',
})
export class BaseLayoutComponent {
  constructor(@Inject('AUTH_SERVICE_INJECTOR') private authService: AuthService) {
  }

  isMember(): boolean {
    alert(this.authService.isAuthenticated());
    return this.authService.getCurrentUser()?.role == 'admin';
  }

  isAdmin(): boolean {
    return false;
  }
}
