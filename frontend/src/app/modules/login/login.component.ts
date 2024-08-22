import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ FormsModule ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  username: string     = "username"
  password: string     = "password"
  remember_me: boolean = false

  onSubmit(): void {
    alert(`username: ${this.username}\npassword: ${this.password}\nremember me: ${this.remember_me}`)
  }
}
