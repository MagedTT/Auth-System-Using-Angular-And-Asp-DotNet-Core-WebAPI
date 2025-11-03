import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private base_url = 'http://localhost:5225/api/Users';
  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  login(loginObject: any) { 
    return this.http.post<any>(`${this.base_url}/login`, loginObject);
  }

  signUp(user: any) {
    return this.http.post<any>(`${this.base_url}/Register`, user);
  }

  signOut() {
    localStorage.removeItem('token');
    this.router.navigateByUrl('login');
  }

  setToken(tokenValue: string): void {
    localStorage.setItem('token', tokenValue);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
}
