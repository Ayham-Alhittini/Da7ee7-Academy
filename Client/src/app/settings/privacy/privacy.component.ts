import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-privacy',
  templateUrl: './privacy.component.html',
  styleUrls: ['./privacy.component.css']
})
export class PrivacyComponent {

  oldPassword: string;
  newPassword: string;

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  onSumbit() {
    const modal = {
      oldPassword:this.oldPassword, 
      newPassword: this.newPassword
    };

    this.accountService.changePassword(modal).subscribe({
      next: res => {
        if (res['isSuccess']) {
          this.toastr.success("تم التحديث بنجاح");
        } else {
          const errors = res['errors'];
          errors.forEach(error => {
            this.toastr.error(error);
          });
        }
      },
      error: err => {
        console.log(err);
        this.toastr.error("حدث خطأ تاكد من المدخلات");
      }
    })
  }
}
