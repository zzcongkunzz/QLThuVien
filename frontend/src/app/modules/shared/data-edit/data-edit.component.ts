import { Component, Input, output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-data-edit',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './data-edit.component.html',
  styleUrl: './data-edit.component.scss',
})
export class DataEditComponent {
  @Input({ required: true }) formGroup: FormGroup<any> = new FormGroup({});
  @Input({ required: true }) isUpdating = false;

  update = output();
  delete = output();

  isSaving = false;

  submit(event: SubmitEvent) {
    try {
      this.isSaving = true;
      if (event.submitter?.classList.contains('update-btn')) {
        this.delete.emit();
      } else if (event.submitter?.classList.contains('save-btn')) {
        this.update.emit();
        this.isUpdating = false;
      } else throw new Error('Unexpected submission');
    } catch (error) {
      alert(error);
    } finally {
      this.isSaving = false;
    }
  }
}
