import { Component, Input, output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-data-add',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './data-add.component.html',
  styleUrl: './data-add.component.scss',
})
export class DataAddComponent {
  @Input({ required: true }) formGroup: FormGroup<any> = new FormGroup({});

  add = output();

  isSaving = false;

  submit(event: SubmitEvent) {
    try {
      this.isSaving = true;
      this.add.emit();
      this.formGroup.reset();
    } catch (err) {
      alert(err);
    } finally {
      this.isSaving = false;
    }
  }
}
