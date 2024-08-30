import { Component, Input } from '@angular/core';
import { Book } from '../../../../../view-models/book';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../../../../services/book/book.service';

@Component({
  selector: 'app-book-item',
  standalone: true,
  imports: [],
  templateUrl: './book-item.component.html',
  styleUrl: './book-item.component.scss'
})
export class BookItemComponent {
  book!: Book

  constructor(private activatedRoute: ActivatedRoute, private bookService: BookService, private router: Router) { }

  ngOnInit() {
    let bookId: string | null = this.activatedRoute.snapshot.paramMap.get("id");
    if (bookId == null) {
      alert("id missing");
      this.router.navigate(["/"])
      return
    }

    this.bookService.getBook(bookId).subscribe({
      next: response => this.book = response
    , error: err => {
        alert(JSON.stringify(err))
        this.router.navigate(["/"])
      }
    })
  }
}
