import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Category} from "../../../../../view-models/category";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-category-checkbox',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './category-checkbox.component.html',
  styleUrl: './category-checkbox.component.scss'
})
export class CategoryCheckboxComponent {
  @Input({required: true}) category!: Category;
  @Input({required: true}) checked!: boolean;

  @Output() select = new EventEmitter()
  @Output() deselect = new EventEmitter()

  clicked() {
    if (this.checked) this.select.emit();
    else this.deselect.emit();
  }
}
