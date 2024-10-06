import {Component, Inject, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {DOCUMENT} from "@angular/common";
import {BookService} from "../../../../../services/book/book.service";
import {Book} from "../../../../../view-models/book";
import {AuthService} from "../../../../../services/auth/auth.service";
import {HomeBookItemComponent} from "../../components/home-book-item/home-book-item.component";

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [
    HomeBookItemComponent
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit {
  private _localStorage: Storage | undefined;
  recommendedBooks: Book[] = []
  highestRatedBooks: Book[] = []

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private router: Router,
    private bookService: BookService,
    private authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this._localStorage = document.defaultView?.localStorage;
    const loginResultString = this._localStorage?.getItem('loginResult');
    const loginResult = loginResultString ? JSON.parse(loginResultString) : null;
    if (loginResult == null) {
      this.router.navigate(['auth/login']);
    } else {
      const expiresAt = new Date(loginResult.expiresAt);
      const currentTime = new Date();
      console.log("expiresAt", expiresAt);
      console.log("currentTime", currentTime);
      console.log(expiresAt <= currentTime);
      if (loginResult.token == null && expiresAt <= currentTime) {
        this.router.navigate(['auth/login']);
      }
    }

    let user = this.authService.getCurrentUser()
    if (user === undefined) return
    // load recommended books
    this.bookService.getRecommendedBooks(user.id).subscribe({
      next: books => {
        this.recommendedBooks = books;
      },
    })
    // load highest rated books
    this.bookService.getHigestRatedBooks().subscribe({
      next: books => {
        this.highestRatedBooks = books;
      },
    })
  }


}
