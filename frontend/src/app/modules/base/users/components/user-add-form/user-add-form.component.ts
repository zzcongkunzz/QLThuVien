import {Component} from '@angular/core';
import {DataAddComponent} from "../../../../shared/components/data-add/data-add.component";
import {FormGroup} from "@angular/forms";

@Component({
  selector: 'app-user-add-form',
  standalone: true,
  imports: [
    DataAddComponent
  ],
  templateUrl: './user-add-form.component.html',
  styleUrl: './user-add-form.component.scss'
})
export class UserAddFormComponent {
  form: FormGroup = new FormGroup({});
}
