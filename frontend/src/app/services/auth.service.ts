import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private base_url = 'http://localhost:5225/api/Users';
  constructor(private http: HttpClient) { }

  login(loginObject: any) { 
    return this.http.post<any>(`${this.base_url}/login`, loginObject);
  }

  signUp(user: any) {
    return this.http.post<any>(`${this.base_url}/Register`, user);
  }

}
