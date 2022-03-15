import { Injectable } from '@angular/core';
import * as oidc from 'oidc-client';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    userManager: oidc.UserManager;

    constructor() {
        this.userManager = new oidc.UserManager({
            authority: "https://localhost:7000",
            client_id: "angular-app",
            redirect_uri: "http://localhost:4200/callback",
            response_type: "code",
            scope: "openid profile api1.read",
            post_logout_redirect_uri: "http://localhost:4200"
        });
    }
}