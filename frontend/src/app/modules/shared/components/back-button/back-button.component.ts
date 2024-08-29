import {Component, inject} from '@angular/core';
import {RouterLink} from '@angular/router';
import {Location} from "@angular/common";

@Component({
  selector: 'app-back-button',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './back-button.component.html',
  styleUrl: './back-button.component.scss',
})
export class BackButtonComponent {
  private _location = inject(Location);

  back() {
    this._location.back();
  }
}
