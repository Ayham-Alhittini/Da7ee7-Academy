import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-personal-information',
  templateUrl: './personal-information.component.html',
  styleUrls: ['./personal-information.component.css']
})
export class PersonalInformationComponent implements OnInit{

  user: User = {
    email: '',
    firstName: '',
    fullName: '',
    gender: '',
    phoneNumber: '',
    id: '',
    role: '',
    token: '',
  }
  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.accountService.getProfile().subscribe(result => {
      this.user = result;
    });
  }

  submit() {
    const model = {phoneNumber: this.user.phoneNumber, gender: this.user.gender};
    console.log(model);
    this.accountService.updateProfile(model).subscribe({
      next: () => {
        this.toastr.success("تم تحديث البيانات الشخصية بنجاح", "نجاح");
      },
      error: (error) => {
        console.log(error);
        this.toastr.error("حدث خطأ أثناء تحديث البيانات الشخصية", "خطأ");
      }
    });
  }
}
