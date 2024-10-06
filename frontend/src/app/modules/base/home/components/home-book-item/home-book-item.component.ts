import {Component, Input} from '@angular/core';
import {Book} from "../../../../../view-models/book";
import {DecimalPipe, NgOptimizedImage} from "@angular/common";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-home-book-item',
  standalone: true,
  imports: [
    NgOptimizedImage,
    DecimalPipe,
    RouterLink
  ],
  templateUrl: './home-book-item.component.html',
  styleUrls: ['./home-book-item.component.scss', '../../pages/home-page/home-page.component.scss']
})
export class HomeBookItemComponent {
  @Input({required: true}) book!: Book;
}
