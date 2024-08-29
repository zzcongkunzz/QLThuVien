import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BookEdit } from '../../view-models/book-edit';
import { Book } from '../../view-models/book';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../../view-models/paginated-result';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  constructor(private http: HttpClient) { }

  uploadBook(bookInfo: BookEdit) {
    return this.http.post("http://localhost:5228/api/books/add-book", bookInfo);
  }

  getBook(id: string): Observable<Book> {
    return this.http.get<Book>(`http://localhost:5228/api/books/get-book-by-id/${id}`)
  }

  putBook(id: string, bookInfo: BookEdit): Observable<any> {
    return this.http.put(`http://localhost:5228/api/books/update-book/${id}`, bookInfo)
  }

  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>("http://localhost:5228/api/books/get-all-books")
  }

  queryBooks(index: number, size: number, title: string, category: string): Observable<PaginatedResult<Book>> {
    return this.http.get<PaginatedResult<Book>>("http://localhost:5228/api/books/query-books",
                                                { params: new HttpParams().set("pageIndex", index).set("pageSize", size).set("title", title).set("category", category) })
  }
}
