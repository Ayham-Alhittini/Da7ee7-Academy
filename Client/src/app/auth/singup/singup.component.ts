import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-singup',
  templateUrl: './singup.component.html',
  styleUrls: ['./singup.component.css']
})
export class SingupComponent  implements OnInit {

  constructor(private fb: FormBuilder,
     private accountService: AccountService, 
     private router: Router, 
     private toaster: ToastrService){}
  
  singupForm: FormGroup;
  submitted = false;
  loading = false;

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    const strongPasswordRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()-_=+{}[\]:;"'<>,.?/]).{8,}$/;

    this.singupForm = this.fb.group({
      fullName : ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(10)]],
      password: ['', [Validators.required, Validators.pattern(strongPasswordRegex)]]
    });
  }


  onSubmit() {
    this.submitted = true;

    if (this.singupForm.invalid) return;
    
    this.loading = true;
    this.accountService.studentRegister(this.singupForm.value).subscribe({
      next: () => {
        this.toaster.success("أدخل الى الرابط المرسل الى بريدك لاثبات ملكية بريدك", "تأكيد البريد الالكتروتي", {
          timeOut: 6000
        });
        this.loading = false;
  
        this.router.navigateByUrl("/login");
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
