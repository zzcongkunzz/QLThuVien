import {CommonModule, NgFor} from '@angular/common';
import {Component} from '@angular/core';
import {EntryBookComponent} from '../../components/entry-book/entry-book.component';
import {Book} from '../../../../../view-models/book';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [CommonModule, EntryBookComponent, NgFor],
  templateUrl: './search-page.component.html',
  styleUrl: './search-page.component.scss'
})
export class SearchPageComponent {
  exampleBooks: Book[] = [
    {
      id: "A"
      , title: "Book A"
      , authorName: "Book A's Author"
      , description: "A Description"
      , categoryName: "Scifi"
      , imageUrl: "https://bookstoreromanceday.org/wp-content/uploads/2020/08/book-cover-placeholder.png"
      , publishDate: new Date()
      , averageRatings: 4.3
      , publisherName: "NXB A"
      , count: 2
    }
    , {
      id: "B"
      , title: "Book B"
      , authorName: "Book B's Author"
      , description: "B Description"
      , categoryName: "Fantasy"
      , imageUrl: "https://bookstoreromanceday.org/wp-content/uploads/2020/08/book-cover-placeholder.png"
      , publishDate: new Date()
      , averageRatings: 2.3
      , publisherName: "NXB B"
      , count: 2
    }
  ]
  exampleData = ["Fst", "Snd", "Thd"]
}
