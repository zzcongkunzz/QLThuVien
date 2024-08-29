import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Book } from '../../view-models/book-edit';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  constructor(private http: HttpClient) { }

  uploadBook(bookInfo: Book) {
    return this.http.post("http://localhost:5228/api/books/add-book", bookInfo);
  }
}
