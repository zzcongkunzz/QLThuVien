import {EventEmitter, inject, Injectable} from '@angular/core';
import {User} from "../../view-models/user";
import {UserCreate} from "../../view-models/user-create";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private user: User | null = null;
  private http: HttpClient = inject(HttpClient);

  changed = new EventEmitter<User[]>();

  addUser(userInfo: UserCreate) {
    let request = this.http.post("/api/users/add-user", userInfo);
    request.subscribe(() => {
      this.getAllUsers().subscribe(users => {
        this.changed.emit(users);
      })
    });
    return request;
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>("/api/users/get-all-users");
  }
}
