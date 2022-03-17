import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, from, take, finalize } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    private refreshTokenInProgress = false;
    private refreshTokenSubject = new BehaviorSubject<string | null>(null);

    constructor(private authService: AuthService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return from(this.authService.getUser()).pipe(
            switchMap(user => {
                if (user) {
                    req = this.addToken(req, user.access_token);
                    return next.handle(req).pipe(catchError((error: HttpErrorResponse) => {
                        if (error && error.status == 401) {
                            if (this.refreshTokenInProgress) {
                                return this.refreshTokenSubject.pipe(
                                    take(1),
                                    switchMap(() => next.handle(this.addToken(req, user.access_token)))
                                );
                            } else {
                                this.refreshTokenInProgress = true;
                                this.refreshTokenSubject.next(null);

                                return from(this.authService.renewToken()).pipe(
                                    switchMap(user => {
                                        this.refreshTokenSubject.next(user.access_token);
                                        return next.handle(this.addToken(req, user.access_token));
                                    }),
                                    finalize(() => (this.refreshTokenInProgress = false))
                                );
                            }
                        }
                        throw new Error(error.message);
                    }));
                } else {
                    return next.handle(req);
                }
            })
        );
    }

    private addToken(req: HttpRequest<any>, token: string) {
        return req.clone({ headers: req.headers.set('Authorization', `Bearer ${token}`) });;
    }

    private handle401Error(req: HttpRequest<any>, next: HttpHandler) {

    }

}