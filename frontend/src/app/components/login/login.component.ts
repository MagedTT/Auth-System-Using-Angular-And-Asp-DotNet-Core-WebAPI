import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from "@angular/router";
import ValidateForm from '../../helpers/validatorForm';

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

  constructor(private fb: FormBuilder) { }

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

  onSubmit() {
    if (this.loginForm.valid) {
      console.log(this.loginForm.value);
    } else {
      ValidateForm.validateAllFormFields(this.loginForm);
    }
  }
}