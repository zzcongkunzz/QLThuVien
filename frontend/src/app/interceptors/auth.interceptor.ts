import {HttpInterceptorFn} from '@angular/common/http';
import {inject} from "@angular/core";
import {AuthService} from "../services/auth/auth.service";

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = inject(AuthService).getAccessToken();
  if (token) {
    const cloned = req.clone({
      setHeaders: {
        authorization: 'Bearer ' + token,
      },
    });
    return next(cloned);
  } else {
    return next(req);
  }
};
