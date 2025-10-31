import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from "@angular/router";
import ValidateForm from '../../helpers/validatorForm';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [
    RouterLink,
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  isPassword: boolean = true;
  eyeIcon: string = 'fa fa-eye-slash';
  passwordInputType: string = 'password';
  loginForm!: FormGroup;

  constructor(private router: Router, private fb: FormBuilder, private authService: AuthService) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  togglePassword() {
    if (this.isPassword) {
      this.eyeIcon = 'fa fa-eye-slash';
      this.passwordInputType = 'password';
    } else {
      this.eyeIcon = 'fa fa-eye';
      this.passwordInputType = 'text';
    }
    this.isPassword = !this.isPassword;
  }

  onLogin() {
    if (this.loginForm.valid) {
      // console.log(this.loginForm.value);
      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          alert(response.message);
          this.loginForm.reset();
          this.router.navigateByUrl('dashboard');
        },
        error: (err) => {
          alert(err?.error?.message);
        }
      });
    } else {
      ValidateForm.validateAllFormFields(this.loginForm);
    }
  }
}