import {Component} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {BookService} from '../../../../../services/book/book.service';
import {BookEdit} from '../../../../../view-models/book-edit';
import {FormsModule} from '@angular/forms';
import {CategoryService} from '../../../../../services/category/category.service';

@Component({
  selector: 'app-edit-book-page',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-book-page.component.html',
  styleUrl: './edit-book-page.component.scss'
})
export class EditBookPageComponent {
  bookId: string | null = "33"
  bookInfo: BookEdit = {
    title: "",
    description: "",
    authorName: "",
    publisherName: "",
    categoryName: "",
    publishDate: new Date("2000-01-01"),
    count: 0,
  }

  constructor(private activedRoute: ActivatedRoute,
              private router: Router,
              private bookService: BookService,
              private categoryService: CategoryService) {
  }

  ngAfterViewInit() {
    this.categoryService.refreshCategories()
    this.bookId = this.activedRoute.snapshot.paramMap.get("id");
    if (this.bookId == null) {
      alert("id not present")
      this.router.navigate(["/"]);
      return
    }

    this.bookService.getBook(this.bookId).subscribe({
      next: book => {
        this.bookInfo.title = book.title
        this.bookInfo.description = book.description
        this.bookInfo.categoryName = book.categoryName
        this.bookInfo.publisherName = book.publisherName
        this.bookInfo.authorName = book.authorName
        this.bookInfo.publishDate = book.publishDate
        this.bookInfo.count = book.count
      }
      , error: error => {
        alert(JSON.stringify(error))
        this.router.navigate(["/"])
      }
    })
  }

  editBook() {
    if (this.bookId == null) {
      alert("id not pressent")
      return
    }
    this.bookService.putBook(this.bookId, this.bookInfo).subscribe({
      next: _ => alert("Success")
      , error: error => alert(JSON.stringify(error))
    });
  }

  getCategories(): string[] {
    return this.categoryService.categoryNames;
  }
}
