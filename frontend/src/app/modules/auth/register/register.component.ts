import {Component} from '@angular/core';
import {UserCreate} from '../../../view-models/user-create';
import {FormsModule} from '@angular/forms';
import {NgFor} from '@angular/common';
import {AuthService} from '../../../services/auth/auth.service';
import {Router} from "@angular/router";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, NgFor],
  // providers: [AuthService],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  constructor(private authService: AuthService, private router: Router) {
  }

  userInfo: UserCreate =
    {
      email: ""
      , password: ""
      , gender: ""
      , dateOfBirth: new Date()
      , fullName: ""
      , role: "member"
    };
  repeatPassword: string = "";

  onSubmit() {
    if (this.userInfo.password != this.repeatPassword) {
      alert("Miss match password");
    } else {
      this.authService.registerUser(this.userInfo);
      this.router.navigate(["/"]);
    }
  }
}
