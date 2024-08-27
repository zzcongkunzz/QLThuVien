import { CommonModule, NgFor } from '@angular/common';
import { Component } from '@angular/core';
import { EntryBookComponent } from '../../../components/entry-book/entry-book.component';
import { Book } from '../../../../../view-models/book';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [ CommonModule, EntryBookComponent, NgFor ],
  templateUrl: './search-page.component.html',
  styleUrl: './search-page.component.scss'
})
export class SearchPageComponent {
  exampleBooks: Book[] = [
    { id: "A"
    , title: "Book A"
    , authorName: "Book A's Author"
    , description: "A Description"
    , categoryId: "Scifi"
    , imageUrl: "https://bookstoreromanceday.org/wp-content/uploads/2020/08/book-cover-placeholder.png"
    , publishDate: new Date()
    , averageRatings: 4.3
    }
  , { id: "B"
    , title: "Book B"
    , authorName: "Book B's Author"
    , description: "B Description"
    , categoryId: "Fantasy"
    , imageUrl: "https://bookstoreromanceday.org/wp-content/uploads/2020/08/book-cover-placeholder.png"
    , publishDate: new Date()
    , averageRatings: 2.3
    }
  ]
  exampleData = ["Fst", "Snd", "Thd"]
}
