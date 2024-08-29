import {Component, Inject, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {DOCUMENT} from "@angular/common";

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent implements OnInit {
  private _localStorage: Storage | undefined;

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this._localStorage = document.defaultView?.localStorage;
    const loginResultString = this._localStorage?.getItem('loginResult');
    const loginResult = loginResultString ? JSON.parse(loginResultString) : null;
    if (loginResult == null) {
      this.router.navigate(['auth/login']);
    } else {
      const expiresAt = new Date(loginResult.expiresAt);
      const currentTime = new Date();
      console.log("expiresAt", expiresAt);
      console.log("currentTime", currentTime);
      console.log(expiresAt <= currentTime);
      if (loginResult.token == null && expiresAt <= currentTime) {
        this.router.navigate(['auth/login']);
      }
    }
  }


}
