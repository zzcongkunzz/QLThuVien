import {Component, Input} from '@angular/core';
import {Book} from '../../../../../view-models/book';

@Component({
  selector: 'app-entry-book',
  standalone: true,
  imports: [],
  templateUrl: './entry-book.component.html',
  styleUrl: './entry-book.component.scss'
})
export class EntryBookComponent {
  @Input() book: Book = new Book()
}
