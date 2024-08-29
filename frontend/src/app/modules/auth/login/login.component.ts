import {Component, Inject} from '@angular/core';
import { FormsModule } from '@angular/forms';
import {Router} from "@angular/router";
import {AuthService} from "../../../services/auth/auth.service";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ FormsModule ],
  providers: [{ provide: 'AUTH_SERVICE_INJECTOR', useClass: AuthService }],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  username: string     = "username"
  password: string     = "password"
  remember_me: boolean = false

  public dialogTitle: string = '';
  public dialogMessage: string = '';
  public isShowDialog: boolean = false;

  constructor(
    @Inject('AUTH_SERVICE_INJECTOR') private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {
    alert(`username: ${this.username}\npassword: ${this.password}\nremember me: ${this.remember_me}`)

    this.authService.login('/', {
      email: this.username,
      password: this.password
    })
      .then((response: any) => {
      if (response) {
        if (response.error) {
          this.dialogTitle = 'Login Failed';
          this.dialogMessage = response.error.detail;
          this.isShowDialog = true;
        } else {
          window.location.href = '/';
        }
      }
    });
  }
}
