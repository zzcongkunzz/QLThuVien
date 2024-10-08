import {Inject, Injectable} from '@angular/core';
import {UserCreate} from '../../view-models/user-create';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {User} from "../../view-models/user";
import {DOCUMENT} from "@angular/common";
import {Login} from "../../view-models/login";
import {AuthResult} from "../../view-models/auth-result";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public _response: AuthResult | undefined;
  // public apiUrl = '/api/authentication/login';

  private _localStorage: Storage | undefined;

  private _user: User | undefined;

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private httpClient: HttpClient,
    private router: Router
  ) {
    this._localStorage = document.defaultView?.localStorage;

    if (!this._response) {
      const loginResult: any = this._localStorage?.getItem('loginResult');
      if (loginResult) {
        this._response = JSON.parse(loginResult);
      }
    }
  }

  public registerUser(userInfo: UserCreate) {
    this.httpClient.post<AuthResult | undefined>("/api/authentication/register", userInfo).subscribe({
      next: response => {
        this._response = response;
        this._localStorage?.setItem('loginResult', JSON.stringify(response));
        this._user = response?.userInformation;
        this._localStorage?.setItem(
          'userInformation',
          JSON.stringify(this._user)
        );
      },
    });
  }


  public async login(
    returnUrl: string,
    model: Login
  ): Promise<AuthResult | undefined> {
    this._localStorage?.setItem('returnUrl', returnUrl);
    return this.httpClient
      .post<AuthResult>("/api/authentication/login", model)
      .toPromise()
      .then((response) => {
        console.log(response);
        this._response = response;
        this._localStorage?.setItem('loginResult', JSON.stringify(response));
        this._user = response?.userInformation;
        this._localStorage?.setItem(
          'userInformation',
          JSON.stringify(this._user)
        );
        return response;
      })
      .catch((error) => {
        return error;
      });
  }

  public isLoggedIn(): boolean {
    return (
      this._response != null &&
      this._response.token != null &&
      this._response.expiresAt != null
    );
  }

  public isAuthenticated() {
    return this.isLoggedIn();
  }

  public getAccessToken() {
    return this._response?.token;
  }

  public logout(): boolean {
    this._response = undefined;
    this._localStorage?.removeItem('loginResult');
    this._localStorage?.removeItem('returnUrl');
    this._localStorage?.removeItem('userInformation');
    this._user = undefined;
    this.router.navigate(['auth', 'login']);
    return true;
  }

  public getCurrentUser(): User | undefined {
    const userJSON = this._localStorage?.getItem('userInformation');
    const user: any = userJSON ? JSON.parse(userJSON) : null;
    return user ? user : null;
  }

  public isAdmin(): boolean {
    const userJSON = this._localStorage?.getItem('userInformation');
    const user: any = userJSON ? JSON.parse(userJSON) : null;

    return user?.role === 'admin';
  }

  public isMember(): boolean {
    const userJSON = this._localStorage?.getItem('userInformation');
    const user: any = userJSON ? JSON.parse(userJSON) : null;

    return user?.role === 'member';
  }


}


