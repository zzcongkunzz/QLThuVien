import {Component} from '@angular/core';
import {UserEditFormComponent} from "../../components/user-edit-form/user-edit-form.component";
import {ChangePasswordFormComponent} from "../../components/change-password-form/change-password-form.component";

@Component({
  selector: 'app-user-profile-page',
  standalone: true,
  imports: [
    UserEditFormComponent,
    ChangePasswordFormComponent
  ],
  templateUrl: './user-profile-page.component.html',
  styleUrl: './user-profile-page.component.scss'
})
export class UserProfilePageComponent {

}
