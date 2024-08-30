import {Component, Input} from '@angular/core';
import {Book} from '../../../../../view-models/book';
import { NgIf } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-entry-book',
  standalone: true,
  imports: [ NgIf, RouterModule ],
  templateUrl: './entry-book.component.html',
  styleUrl: './entry-book.component.scss'
})
export class EntryBookComponent {
  @Input() book: Book = new Book()

  getEditLink(): string {
    return `/books/edit/${this.book.id}`
  }

  getInfoLink(): string {
    return `/books/detail/${this.book.id}`
  }

  ngOnInit() {
    this.book.publishDate = new Date(this.book.publishDate)
  }
}
