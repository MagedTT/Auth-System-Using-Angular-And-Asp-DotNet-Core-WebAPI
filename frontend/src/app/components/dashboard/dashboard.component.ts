import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  // users: any = [];
  constructor(private authService: AuthService) { }

  // ngOnInit(): void {
  //   this.a
  // }

  logout() {
    this.authService.signOut();
  }
}
