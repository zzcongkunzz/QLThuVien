import {DOCUMENT} from '@angular/common';
import {Inject, Injectable} from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanLoad,
  Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree,
} from '@angular/router';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthenticatedUserGuard implements CanActivate, CanLoad {
  private localStorage: Storage | undefined;

  public constructor(
    @Inject(DOCUMENT) private document: Document,
    protected router: Router
  ) {
    this.localStorage = document.defaultView?.localStorage;
  }

  /*
   * Whether user can access to authenticated modules or not.
   * */
  public canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    // if authenticated
    if (this.hasUserAuthenticated()) {
      if (this.isManager()) {
        return true;
      }

      // if not admin authenticated
      this.router.navigate(['error/403']);
      return false;
    }

    // if not authenticated
    this.router.navigate(['auth/login'], {queryParams: {returnUrl: state.url}});
    return false;
  }

  /*
   * Whether module can be loaded or not.
   * */
  public canLoad(
    route: Route,
    segments: UrlSegment[]
  ): Observable<boolean> | Promise<boolean> | boolean {
    return this.hasUserAuthenticated();
  }

  /*
   * Check whether user has authenticated into system or not.
   * */
  protected hasUserAuthenticated(): boolean {
    const loginResult = this.localStorage?.getItem('loginResult');
    return !!loginResult;
  }

  private isManager(): boolean {
    const userInformation = this.localStorage?.getItem('userInformation');
    if (userInformation) {
      const user = JSON.parse(userInformation);
      return user.roles.includes('Admin') || user.roles.includes('Editor');
    }

    return false;
  }
}
