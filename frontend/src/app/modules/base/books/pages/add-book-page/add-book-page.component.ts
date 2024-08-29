import { Component } from '@angular/core';
import { Book } from '../../../../../view-models/book-edit';
import { CategoryService } from '../../../../../services/category/category.service';
import { NgFor } from '@angular/common';
import { BookService } from '../../../../../services/book/book.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-book-page',
  standalone: true,
  imports: [ NgFor, FormsModule ],
  templateUrl: './add-book-page.component.html',
  styleUrl: './add-book-page.component.scss'
})
export class AddBookPageComponent {
  bookInfo: Book = {
    title: ""
  , description: ""
  , authorName: ""
  , publisherName: ""
  , categoryName: ""
  , publishDate: new Date("2000-01-01")
  , count: 0
  }

  constructor(private categoryService: CategoryService
             ,private bookService: BookService) {
  }

  ngAfterViewInit() {
    this.categoryService.refreshCategories()
  }

  getCategories(): string[] {
    return this.categoryService.categories;
  }

  addBook() {
    this.bookService.uploadBook(this.bookInfo).subscribe({
      next: _ => {
        alert("Successful")
        this.bookInfo =
        { title: ""
        , description: ""
        , authorName: ""
        , publisherName: ""
        , categoryName: ""
        , publishDate: new Date("2000-01-01")
        , count: 0
        }
      }
    , error: error => alert(JSON.stringify(error))
    })
  }
}
