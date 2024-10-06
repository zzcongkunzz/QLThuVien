import {CommonModule, NgFor} from '@angular/common';
import {Component} from '@angular/core';
import {EntryBookComponent} from '../../components/entry-book/entry-book.component';
import {Book} from '../../../../../view-models/book';
import {BookService} from '../../../../../services/book/book.service';
import {FormsModule} from '@angular/forms';
import {CategoryService} from '../../../../../services/category/category.service';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [CommonModule, EntryBookComponent, NgFor, FormsModule],
  templateUrl: './search-page.component.html',
  styleUrl: './search-page.component.scss'
})
export class SearchPageComponent {
  searchQuery: string = ""
  searchCategory: string = ""

  allBooks: Book[] = []

  hasNextPage: boolean = false
  hasPrevPage: boolean = false

  page: number = 1

  constructor(private bookService: BookService, private categoryService: CategoryService) {
  }

  ngOnInit() {
    this.page = 1
    this.search()
    // this.bookService.getAllBooks().subscribe({
    //   next: res => this.allBooks = res
    // , error: err => alert(JSON.stringify(err))
    // })
  }

  search() {
    this.bookService.queryBooks(this.page, 6, this.searchQuery, this.searchCategory).subscribe({
      next: res => {
        this.allBooks = res.items;
        this.hasNextPage = res.hasNextPage;
        this.hasPrevPage = res.hasPreviousPage;
      }
      , error: err => alert(JSON.stringify(err))
    })
  }

  ngAfterViewInit() {
    this.categoryService.refreshCategories()
  }

  getCategories(): string[] {
    return this.categoryService.categories;
  }

  nextPage() {
    if (!this.hasNextPage) return;
    this.page += 1
    this.search()
  }

  previousPage() {
    if (!this.hasPrevPage) return;
    this.page -= 1
    this.search()
  }
}
