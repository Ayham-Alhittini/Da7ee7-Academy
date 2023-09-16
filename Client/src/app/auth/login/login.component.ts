import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router){}

  ngOnInit(): void {
    this.initializeForm();
  }
  submitted = false;
  loginForm: FormGroup;

  initializeForm() {
    this.loginForm = this.fb.group({
      loginProvider: ['', Validators.required],
      password : ['', [Validators.required]]
    });
  }

  onSubmit() {
    this.submitted = true;
    if (this.loginForm.invalid) return;
    
    this.accountService.login(this.loginForm.value).subscribe(
      response => {
        this.router.navigateByUrl("/home");
      }
    );
    
  }
}
