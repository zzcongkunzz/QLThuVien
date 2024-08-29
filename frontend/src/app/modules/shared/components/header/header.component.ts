import {Component, Inject, OnInit} from '@angular/core';
import {DOCUMENT} from "@angular/common";
import {Router} from "@angular/router";
import {User} from "../../../../view-models/user";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [ FormsModule ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit{
  private _localStorage: Storage | undefined;
  public user : User | undefined;

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private router: Router
  ) {}

  ngOnInit(): void {
    this._localStorage = document.defaultView?.localStorage;
    const userString = this._localStorage?.getItem('userInformation');
    this.user = userString ? JSON.parse(userString) : undefined;
  }

  dangXuat(): void{
    this._localStorage?.clear();
    this.router.navigate(['auth/login']);
  }
}
