import {Component} from '@angular/core';
import {BackButtonComponent} from "../../../../shared/components/back-button/back-button.component";
import {UserEditFormComponent} from "../../components/user-edit-form/user-edit-form.component";
import {ChangePasswordFormComponent} from "../../components/change-password-form/change-password-form.component";

@Component({
  selector: 'app-user-edit-page',
  standalone: true,
  imports: [
    BackButtonComponent,
    UserEditFormComponent,
    ChangePasswordFormComponent
  ],
  templateUrl: './user-edit-page.component.html',
  styleUrl: './user-edit-page.component.scss'
})
export class UserEditPageComponent {

}
