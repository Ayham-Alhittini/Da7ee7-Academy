import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { SalePoint } from '../models/sale-point';
import { AccountService } from '../services/account.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AdminService } from '../services/admin.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-sale-points-view',
  templateUrl: './sale-points-view.component.html',
  styleUrls: ['./sale-points-view.component.css']
})
export class SalePointsViewComponent implements OnInit{
  
  activeGov = 'all';

  addSalePointForm: FormGroup;
  constructor(private userService: UserService, 
    public accountService: AccountService, 
    private adminService: AdminService, 
    private toastr: ToastrService,
    private fb: FormBuilder) {}
    
    salePointsSrc: SalePoint[] = [];
    salePoints: SalePoint[] = [];
    governorates = [];

  ngOnInit(): void {
    this.loadSalePoints();
    this.initializeForm();
    this.governorates = this.userService.getGovernorates();
  }

  loadSalePoints() {
    this.userService.getSalePoints().subscribe(result => {
      this.salePointsSrc = result;
      this.setSalePointByGov();
    })
  }

  initializeForm() {
    const urlPattern = /^(https?|ftp):\/\/[^\s/$.?#].[^\s]*$/;
    this.addSalePointForm = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      governorate: ['عمان', Validators.required],
      addressUrl: ['', [Validators.required, Validators.pattern(urlPattern)]],
      phoneNumber: ['', Validators.required]
    });
  }

  onAddSaleSubmit() {
    if (this.addSalePointForm.invalid) {
      return;
    }
      
    this.adminService.addSalePoint(this.addSalePointForm.value).subscribe({
      next: () => {
        this.toastr.success("Added Successfly");
        this.onReset();
        this.loadSalePoints();
      },
      error: err => {
        console.log(err);
      }
    });
  }

  onSelectedGovChange(event: any) {{
    const newGov = event.target.value;
    this.activeGov = newGov;
    
    this.setSalePointByGov();
  }}

  setSalePointByGov() {
    var tempSalePoints = this.salePointsSrc.slice();

    if (this.activeGov !== 'all') {
      tempSalePoints = tempSalePoints.filter(sp => sp.governorate === this.activeGov);
    }

    this.salePoints = tempSalePoints;
  }

  onSalePointDelete(salePoint: SalePoint) {
    const response = confirm("Are you sure");
    if (response) {
      this.adminService.deleteSalePoint(salePoint.id).subscribe(() => {
        this.toastr.warning("Deleted successfuly", null, {
          positionClass: 'toast-bottom-center'
        });
        this.loadSalePoints();
      })
    }
  }

  onReset() {
    this.addSalePointForm.reset({
      governorate: 'عمان'
    });
  }
}
