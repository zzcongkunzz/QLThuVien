import {Component} from '@angular/core';
import {UserEditFormComponent} from "../../components/user-edit-form/user-edit-form.component";
import {ChangePasswordFormComponent} from "../../components/change-password-form/change-password-form.component";
import {FavoriteCategoriesComponent} from "../../components/favorite-categories/favorite-categories.component";

@Component({
  selector: 'app-user-profile-page',
  standalone: true,
  imports: [
    UserEditFormComponent,
    ChangePasswordFormComponent,
    FavoriteCategoriesComponent
  ],
  templateUrl: './user-profile-page.component.html',
  styleUrl: './user-profile-page.component.scss'
})
export class UserProfilePageComponent {

}
