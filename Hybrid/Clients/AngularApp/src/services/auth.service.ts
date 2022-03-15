import { Injectable } from '@angular/core';
import * as oidc from 'oidc-client';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    config = {
        authorith: "https://localhost:7000",
        client_id: "angular-app",
        redirect_uri: "https://localhost:4200/callback",
        response_type: "code",
        scope: "openid profile email api1.read",
        post_logout_redirect_uri: "https://localhost:4200"
    };

    userManager;

    constructor() {
        this.userManager = new oidc.UserManager(this.config);
    }
}