import {Component, inject, Input} from '@angular/core';
import {Book} from '../../../../../view-models/book';
import {CommonModule, NgIf} from '@angular/common';
import {RouterModule} from '@angular/router';
import {AuthService} from "../../../../../services/auth/auth.service";

@Component({
  selector: 'app-entry-book',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './entry-book.component.html',
  styleUrl: './entry-book.component.scss'
})
export class EntryBookComponent {
  @Input() book: Book = new Book()
  authService: AuthService = inject(AuthService);

  getEditLink(): string {
    return `/books/edit/${this.book.id}`
  }

  getInfoLink(): string {
    return `/books/detail/${this.book.id}`
  }

  ngOnInit() {
    this.book.publishDate = new Date(this.book.publishDate)
  }

  protected readonly NgIf = NgIf;
}
