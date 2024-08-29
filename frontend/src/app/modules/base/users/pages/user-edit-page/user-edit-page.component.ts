import {Component} from '@angular/core';
import {BackButtonComponent} from "../../../../shared/components/back-button/back-button.component";
import {UserEditFormComponent} from "../../components/user-edit-form/user-edit-form.component";

@Component({
  selector: 'app-user-edit-page',
  standalone: true,
  imports: [
    BackButtonComponent,
    UserEditFormComponent
  ],
  templateUrl: './user-edit-page.component.html',
  styleUrl: './user-edit-page.component.scss'
})
export class UserEditPageComponent {

}
