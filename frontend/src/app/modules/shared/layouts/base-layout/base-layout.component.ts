import {Component, Inject} from '@angular/core';
import {HeaderComponent} from '../../components/header/header.component';
import {RouterModule, RouterOutlet} from '@angular/router';
import {CommonModule} from '@angular/common';
import { AuthService } from '../../../../services/auth/auth.service';
import { AuthModule } from '../../../auth/auth.module';

@Component({
  selector: 'app-base-layout',
  standalone: true,
  imports: [HeaderComponent, RouterOutlet, RouterModule, CommonModule, AuthModule],
  templateUrl: './base-layout.component.html',
  styleUrl: './base-layout.component.scss',
})
export class BaseLayoutComponent {
  // NOTE: Current AuthService have no way of telling if the user is admin or member
  constructor(@Inject('AUTH_SERVICE_INJECTOR') private authService: AuthService) {
  }

  isMember(): boolean {
    return this.authService.isMember();
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

}
