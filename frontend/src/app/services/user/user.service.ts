import {EventEmitter, inject, Injectable} from '@angular/core';
import {User} from "../../view-models/user";
import {UserCreate} from "../../view-models/user-create";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {UserEdit} from "../../view-models/user-edit";
import {ChangePassword} from "../../view-models/change-password";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private user: User | null = null;
  private http: HttpClient = inject(HttpClient);

  changed = new EventEmitter<User[]>();

  addUser(userCreate: UserCreate) {
    let request = this.http.post("/api/users/add-user", userCreate);
    request.subscribe({
      next: () => {
        this.triggerChange();
      },
      error: (err: Error) => {
        alert(JSON.stringify(err));
      }, complete: () => {
        this.triggerChange();
      }
    });
    return request;
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>("/api/users/get-all-users");
  }

  getUserById(id: string): Observable<User> {
    return this.http.get<User>("/api/users/get-user-by-id/" + id);
  }

  updateUser(id: string, userEdit: UserEdit) {
    let request = this.http.put("/api/users/update-user/" + id, userEdit);
    request.subscribe({
      error: (err: Error) => {
        console.log(JSON.stringify(err));
      },
      next: () => {
        this.triggerChange();
      },
      complete: () => {
        this.triggerChange();
      }
    });
    return request;
  }

  changePassword(id: string, changePassword: ChangePassword) {
    let request = this.http.put("/api/users/change-password/" + id, changePassword);
    request.subscribe({
      error: (err: Error) => {
        alert(JSON.stringify(err));
      },
    });
    return request;
  }

  deleteUser(id: string) {
    let request = this.http.delete("/api/users/delete-user/" + id);
    request.subscribe({
      error: (err: Error) => {
        console.log(JSON.stringify(err));
      },
      next: () => {
        this.triggerChange();
      },
      complete: () => {
        this.triggerChange();
      }
    });
    return request;
  }

  triggerChange() {
    this.getAllUsers().subscribe(users => {
      this.changed.emit(users);
    })
  }
}
