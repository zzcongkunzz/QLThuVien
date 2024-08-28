import {Injectable} from '@angular/core';
import {UserCreate} from '../view-models/user-create';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient, private router: Router) {
  }

  public registerUser(userInfo: UserCreate) {
    this.http.post("/api/users", userInfo).subscribe({
      next: respond => {
        this.router.navigate(["/"]);
      },
      error: error => alert(JSON.stringify(error.error))
    });
  }
}
