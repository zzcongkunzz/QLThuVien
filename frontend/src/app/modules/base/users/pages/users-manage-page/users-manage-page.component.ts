import {Component} from '@angular/core';

import {UserAddFormComponent} from "../../components/user-add-form/user-add-form.component";

@Component({
  selector: 'app-users-manage-page',
  standalone: true,
  imports: [
    UserAddFormComponent
  ],
  templateUrl: './users-manage-page.component.html',
  styleUrl: './users-manage-page.component.scss'
})
export class UsersManagePageComponent {

}
