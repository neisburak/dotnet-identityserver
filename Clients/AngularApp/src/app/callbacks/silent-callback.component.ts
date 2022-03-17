import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserManager } from 'oidc-client';

@Component({
    selector: 'silent-callback',
    template: '',
    styles: []
})
export class SilentCallbackComponent implements OnInit {
    constructor(private router: Router) { }

    ngOnInit(): void {
        new UserManager({ response_mode: 'query' }).signinSilentCallback().then(() => {
            this.router.navigate(['/']);
        }, error => {
            console.error(error);
        });
    }
}