import {Component, inject, OnInit} from '@angular/core';
import {User} from "../../../../../view-models/user";
import {DateOnlyPipe} from "../../../../../pipes/date-only/date-only.pipe";
import {GenderPipe} from "../../../../../pipes/gender/gender.pipe";
import {RolePipe} from "../../../../../pipes/role/role.pipe";
import {DateTimePipe} from "../../../../../pipes/date-time/date-time.pipe";
import {UserService} from "../../../../../services/user/user.service";

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
export class UserListComponent implements OnInit {
  private userService: UserService = inject(UserService);

  users: User[] = []

  ngOnInit(): void {
    this.userService.changed.subscribe((users: User[]) => {
      this.users = users;
    });
  }
}
