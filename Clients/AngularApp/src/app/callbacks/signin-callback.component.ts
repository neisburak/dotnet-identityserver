import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserManager } from 'oidc-client';

@Component({
    selector: 'signin-callback-component',
    template: '',
    styles: []
})
export class SigninCallbackComponent implements OnInit {

    constructor(private router: Router) { }

    ngOnInit(): void {
        new UserManager({ response_mode: 'query' }).signinRedirectCallback().then(() => {
            this.router.navigate(['/'], {});
        }, error => {
            console.error(error);
        });
    }
}