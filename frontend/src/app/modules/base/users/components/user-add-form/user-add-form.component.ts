import {Component, inject} from '@angular/core';
import {DataAddComponent} from "../../../../shared/components/data-add/data-add.component";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {UserService} from "../../../../../services/user/user.service";

@Component({
  selector: 'app-user-add-form',
  standalone: true,
  imports: [
    DataAddComponent,
    ReactiveFormsModule
  ],
  templateUrl: './user-add-form.component.html',
  styleUrl: './user-add-form.component.scss'
})
export class UserAddFormComponent {
  private fb = inject(FormBuilder);
  private userService: UserService = inject(UserService);

  formGroup: FormGroup = this.fb.group({
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    role: ['', Validators.required],
    gender: ['', Validators.required],
    password: ['', Validators.required],
    dateOfBirth: [new Date(), Validators.required],
  });

  onAdd() {
    this.userService.addUser({
      email: this.formGroup.controls["email"].value,
      fullName: this.formGroup.controls["fullName"].value,
      role: this.formGroup.controls["role"].value,
      gender: this.formGroup.controls["gender"].value,
      dateOfBirth: this.formGroup.controls["dateOfBirth"].value,
      password: this.formGroup.controls["password"].value,
    });
  }
}
