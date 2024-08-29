import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-edit-book-page',
  standalone: true,
  imports: [],
  templateUrl: './edit-book-page.component.html',
  styleUrl: './edit-book-page.component.scss'
})
export class EditBookPageComponent {
  bookId: string | null = "33"
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.bookId = this.route.snapshot.paramMap.get("id");
  }
}
