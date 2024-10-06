import {Component, EventEmitter, Input, Output, output} from '@angular/core';
import {FormGroup, ReactiveFormsModule} from '@angular/forms';

@Component({
  selector: 'app-data-edit',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './data-edit.component.html',
  styleUrl: './data-edit.component.scss',
})
export class DataEditComponent {
  @Input({required: true}) formGroup: FormGroup<any> = new FormGroup({});
  @Input() isUpdating = false;
  @Output() isUpdatingChange = new EventEmitter<boolean>();

  update = output();
  delete = output();

  isSaving = false;

  submit(event: SubmitEvent) {
    if (this.isSaving) {
      // alert("Cannot submit another request while saving!");
      return;
    }
    try {
      this.isSaving = true;
      if (event.submitter?.classList.contains('delete-btn')) {
        this.delete.emit();
      } else if (event.submitter?.classList.contains('save-btn')) {
        this.update.emit();
        this.isUpdating = false;
        this.isUpdatingChange.emit(false);
      } else alert("Unexpected submission");
    } catch (error) {
      // alert(error);
    } finally {
      this.isSaving = false;
    }
  }

  enableUpdate() {
    this.isUpdating = true;
    this.isUpdatingChange.emit(true);
  }
}
