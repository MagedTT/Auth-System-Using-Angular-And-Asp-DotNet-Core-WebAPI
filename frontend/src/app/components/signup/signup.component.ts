import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import validateForm from '../../helpers/validatorForm';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';


@Component({
  selector: 'app-signup',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent implements OnInit {
  isPassword: boolean = true;
  eyeIcon: string = 'fa fa-eye-slash';
  passwordInputType: string = 'password';

  signUpForm!: FormGroup;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService,
    private notifyService: NotificationService) { }
  
  ngOnInit(): void {
    this.signUpForm = this.fb.group({
      firstname: ['', [Validators.required, Validators.minLength(3)]],
      lastname: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(8)]],
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

  onSignup() {
    if (this.signUpForm.valid) {
      // console.log(this.signUpForm.value);
      this.authService.signUp(this.signUpForm.value).subscribe({
        next: (response) => {
          this.notifyService.showSuccess(response.message);
          this.signUpForm.reset();
          this.router.navigateByUrl('login');
        },
        error: (err) => {
          this.notifyService.showError(err?.error?.message);
        }
      });
    } else {
      validateForm.validateAllFormFields(this.signUpForm);
    }
  }
}
