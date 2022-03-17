import { Component, OnInit } from '@angular/core';
import { User } from 'oidc-client';
import { Product } from '../models/product.model';
import { ApiService } from '../services/api.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  currentUser: User | null = null;
  items: Product[] = [];

  get currentUserJson() {
    if (this.currentUser) {
      return JSON.stringify(this.currentUser, null, 2);
    }
    return '';
  }

  constructor(private authService: AuthService, private apiService: ApiService) { }

  async ngOnInit() {
    this.currentUser = await this.authService.getUser();
  }

  onLogin = () => this.authService.login();

  onLogout = () => this.authService.logout()

  onRenewToken = async () => {
    this.currentUser = await this.authService.renewToken();
  }

  getProducts = () => {
    this.apiService.get().subscribe(res => {
      this.items = res;
    });
  }

}
