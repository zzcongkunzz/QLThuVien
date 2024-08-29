import {Component, inject, OnInit} from '@angular/core';
import {DataAddComponent} from "../../../../shared/components/data-add/data-add.component";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {DataEditComponent} from "../../../../shared/components/data-edit/data-edit.component";
import {UserService} from "../../../../../services/user/user.service";
import {ActivatedRoute} from "@angular/router";
import {User} from "../../../../../view-models/user";
import {Location} from "@angular/common";
import {RolePipe} from "../../../../../pipes/role/role.pipe";

@Component({
  selector: 'app-user-edit-form',
  standalone: true,
  imports: [
    DataAddComponent,
    ReactiveFormsModule,
    DataEditComponent,
    RolePipe
  ],
  templateUrl: './user-edit-form.component.html',
  styleUrl: './user-edit-form.component.scss'
})
export class UserEditFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private _userService: UserService = inject(UserService);
  private _activeRoute: ActivatedRoute = inject(ActivatedRoute);
  private _location: Location = inject(Location);

  isUpdating: boolean = false;

  onIsUpdatingChange(value: boolean) {
    this.isUpdating = value;
    if (this.isUpdating) {
      this.formGroup.controls["email"].enable();
      this.formGroup.controls["fullName"].enable();
      this.formGroup.controls["dateOfBirth"].enable();
      this.formGroup.controls["gender"].enable();
    } else {
      this.formGroup.controls["email"].disable();
      this.formGroup.controls["fullName"].disable();
      this.formGroup.controls["dateOfBirth"].disable();
      this.formGroup.controls["gender"].disable();
    }
  }

  user: User | undefined;

  formGroup: FormGroup = this.fb.group({
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    gender: ['', Validators.required],
    dateOfBirth: [new Date(), Validators.required],
  });

  ngOnInit(): void {
    const id = this._activeRoute.snapshot.params['id'];

    if (typeof (id) !== 'string')
      alert('id must be a string');
    else
      this.loadData(id);
  }

  onUpdate() {
    if (!this.user) {
      alert("User hasn't been loaded yet!");
      return;
    }
    this._userService.updateUser(this.user.id, {
      email: this.formGroup.value.email,
      fullName: this.formGroup.value.fullName,
      gender: this.formGroup.value.gender,
      dateOfBirth: this.formGroup.value.dateOfBirth,
    }).subscribe({
      complete: () => {
        if (!this.user) {
          alert("User undefined!");
          return;
        } else alert("Update succesful.")
        this.loadData(this.user.id);
      }
    });
  }

  onDelete() {
    if (!this.user) {
      alert("User hasn't been loaded yet!");
      return;
    }
    this._userService.deleteUser(this.user.id).subscribe({
      complete: () => {
        alert("User has been deleted");
        this._location.back();
      }
    })
  }

  loadData(id: string) {
    this._userService.getUserById(id).subscribe({
      next: user => {
        this.user = user;
        this.formGroup.controls["email"].setValue(user.email);
        this.formGroup.controls["fullName"].setValue(user.fullName);
        this.formGroup.controls["dateOfBirth"].setValue(user.dateOfBirth);
        this.formGroup.controls["gender"].setValue(user.gender);
      },
    });
  }
}
