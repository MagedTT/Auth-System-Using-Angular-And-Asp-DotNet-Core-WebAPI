import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import validateForm from '../../helpers/validatorForm';


@Component({
  selector: 'app-signup',
  imports: [
    RouterLink,
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

  constructor(private fb: FormBuilder) { }
  
  ngOnInit(): void {
    this.signUpForm = this.fb.group({
      firstname: ['', [Validators.required, Validators.minLength(3)]],
      lastname: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', Validators.required, Validators.minLength(8)],
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

  onSubmit() {
    if (this.signUpForm.valid) {
      console.log(this.signUpForm.value);
    } else {
      validateForm.validateAllFormFields(this.signUpForm);
    }
  }
}
