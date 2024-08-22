import {Component} from '@angular/core';
import {HeaderComponent} from '../../components/header/header.component';
import {RouterModule, RouterOutlet} from '@angular/router';
import {User} from '../../../../view-models/user';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-base-layout',
  standalone: true,
  imports: [HeaderComponent, RouterOutlet, RouterModule, CommonModule],
  templateUrl: './base-layout.component.html',
  styleUrl: './base-layout.component.scss',
})
export class BaseLayoutComponent {
  user: User = new User();
}
