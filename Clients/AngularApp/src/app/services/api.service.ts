import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Product } from '../models/product.model';

@Injectable({
    providedIn: 'root'
})
export class ApiService {
    constructor(private http: HttpClient) { }

    get = () => {
        return this.http.get<Product[]>(`${environment.apiRoot}/items`);
    }
}