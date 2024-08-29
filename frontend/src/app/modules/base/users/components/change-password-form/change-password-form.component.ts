import {Component, inject, Input, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {UserService} from "../../../../../services/user/user.service";
import {ActivatedRoute} from "@angular/router";
import {AuthService} from "../../../../../services/auth/auth.service";

@Component({
  selector: 'app-change-password-form',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './change-password-form.component.html',
  styleUrl: './change-password-form.component.scss'
})
export class ChangePasswordFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private _userService: UserService = inject(UserService);
  private _activeRoute: ActivatedRoute = inject(ActivatedRoute);
  private _authService: AuthService = inject(AuthService);

  @Input() isProfile = false;

  userId: string | undefined;
  fieldType: "password" | "text" = "password";
  visibilityAction: "Ẩn" | "Hiện" = "Hiện";

  formGroup: FormGroup = this.fb.group({
    currentPassword: ['', [Validators.required, Validators.minLength(6)]],
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
  })

  ngOnInit(): void {
    if (this.isProfile) this.userId = this._authService.getCurrentUser()?.id;
    else this.userId = this._activeRoute.snapshot.params['id'];
  }

  onSubmit() {
    if (this.userId === undefined) {
      alert("userId not loaded yet");
      return;
    }
    this._userService.changePassword(this.userId, {
      currentPassword: this.formGroup.value.currentPassword,
      newPassword: this.formGroup.value.newPassword,
    }).subscribe({
      complete() {
        alert("Password changed successfully!");
      }
    })
  }

  changeFieldType() {
    if (this.fieldType == "password") {
      this.fieldType = "text";
      this.visibilityAction = "Ẩn";
    } else {
      this.fieldType = "password";
      this.visibilityAction = "Hiện";
    }
  }
}
