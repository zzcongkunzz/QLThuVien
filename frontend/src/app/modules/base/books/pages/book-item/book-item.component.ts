import {Component, OnInit} from '@angular/core';
import {Book} from '../../../../../view-models/book';
import {ActivatedRoute, Router} from '@angular/router';
import {BookService} from '../../../../../services/book/book.service';
import {DecimalPipe} from "@angular/common";
import {AuthService} from "../../../../../services/auth/auth.service";
import {FormsModule} from "@angular/forms";
import {BackButtonComponent} from "../../../../shared/components/back-button/back-button.component";
import {HomeBookItemComponent} from "../../../home/components/home-book-item/home-book-item.component";

@Component({
  selector: 'app-book-item',
  standalone: true,
  imports: [
    DecimalPipe,
    FormsModule,
    BackButtonComponent,
    HomeBookItemComponent
  ],
  templateUrl: './book-item.component.html',
  styleUrls: ['./book-item.component.scss', '../../../home/pages/home-page/home-page.component.scss']
})
export class BookItemComponent implements OnInit {
  book!: Book;
  ratingValue: number | undefined;
  similarBooks: Book[] = []

  constructor(private activatedRoute: ActivatedRoute, private bookService: BookService,
              private router: Router, private authService: AuthService) {
  }

  ngOnInit() {
    this.loadBook()
  }

  loadBook() {
    let bookId = this.activatedRoute.snapshot.paramMap.get("id");
    if (bookId == null) {
      this.router.navigate(["/"])
      return
    }

    this.bookService.getBook(bookId).subscribe({
      next: response => {
        this.book = response
      }
      , error: _ => {
        // alert(JSON.stringify(err))
        this.router.navigate(["/"])
      }
    })

    this.bookService.getSimilarBooks(bookId).subscribe({
      next: (books: Book[]) => {
        this.similarBooks = books;
      }
    })
  }

  giveRating() {
    let user = this.authService.getCurrentUser()
    if (user === undefined) {
      // alert("undefined user")
      return
    }
    if (this.ratingValue === undefined) {
      // alert("undefined value")
      return
    }
    this.bookService.giveRating({
      bookId: this.book.id,
      userId: user.id,
      value: this.ratingValue
    }).subscribe({
      next: value => {
        this.book.averageRating = value;
      },
      error: error => alert(JSON.stringify(error))
    })
  }
}
