import {Component} from '@angular/core';
import {User} from "../../../../../view-models/user";
import {DateOnlyPipe} from "../../../../../pipes/date-only/date-only.pipe";
import {GenderPipe} from "../../../../../pipes/gender/gender.pipe";
import {RolePipe} from "../../../../../pipes/role/role.pipe";
import {DateTimePipe} from "../../../../../pipes/date-time/date-time.pipe";

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    DateOnlyPipe,
    GenderPipe,
    RolePipe,
    DateTimePipe
  ],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent {
  users: User[] = [
    new User(),
    new User(),
    new User(),
  ]
}
