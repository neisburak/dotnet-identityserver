import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  status: string = "";

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.userManager.getUser().then((user) => {
      if (user) {
        console.log(user);
        this.status = "Welcome";
      } else {
        this.status = "User not logged in.";
      }
    });
  }

  login() {
    console.log('asd');
    this.authService.userManager.signinRedirect();
  }

  logout() {
    this.authService.userManager.signoutRedirect();
  }

}
