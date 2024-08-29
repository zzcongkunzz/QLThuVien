import {Component, Inject, OnInit} from '@angular/core';
import {DOCUMENT} from "@angular/common";
import {Router} from "@angular/router";
import {User} from "../../../../view-models/user";
import {FormsModule} from "@angular/forms";
import {DateTimePipe} from "../../../../pipes/date-time/date-time.pipe";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [FormsModule, DateTimePipe],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  private _localStorage: Storage | undefined;
  user: User | undefined;
  date: Date = new Date();

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this._localStorage = document.defaultView?.localStorage;
    const userString = this._localStorage?.getItem('userInformation');
    this.user = userString ? JSON.parse(userString) : undefined;
    setInterval(() => {
      this.date = new Date();
    }, 1000)
  }

  dangXuat(): void {
    this._localStorage?.clear();
    this.router.navigate(['auth/login']);
  }
}
