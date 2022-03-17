import { Injectable } from '@angular/core';
import { User, UserManager } from 'oidc-client';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    userManager: UserManager;

    constructor() {
        const settings = {
            authority: environment.stsAuthority,
            client_id: environment.clientId,
            redirect_uri: `${environment.clientRoot}/signin-callback`,
            silent_redirect_uri: `${environment.clientId}/silent-callback`,
            post_logout_redirect_uri: environment.clientRoot,
            response_type: 'code',
            scope: environment.clientScope
        };
        this.userManager = new UserManager(settings);
    }

    getUser = () => this.userManager.getUser();

    login = () => this.userManager.signinRedirect();

    logout = () => this.userManager.signoutRedirect();

    renewToken = () => this.userManager.signinSilent();

    isAuthenticated = async () => {
        return this.checkUser(await this.userManager.getUser());
    }

    private checkUser = (user: User | null) => !!user && !user.expired;
}