import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/services/account.service';
import { AdminService } from 'src/app/services/admin.service';
import { StudentService } from 'src/app/services/student.service';

@Component({
  selector: 'app-teachers',
  templateUrl: './teachers.component.html',
  styleUrls: ['./teachers.component.css']
})
export class TeachersComponent implements OnInit {

  @ViewChild('fileInput') fileInput: ElementRef;

  loading = false;

  addTeacherForm: FormGroup;

  teachers: any = [];

  constructor(private fb: FormBuilder, 
    private toastr: ToastrService, 
    private accountService: AccountService, 
    private adminService: AdminService, 
    public studentService: StudentService){}

  fileValue = null;

  ngOnInit(): void {
    this.loadCourses();
    this.initializeForm();
  }

  loadCourses() {
    this.adminService.getTeachers().subscribe(result => {
      this.teachers = result;
    });
  }

  initializeForm() {
    this.addTeacherForm = this.fb.group({
      fullName : ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(10)]],
      major: ['رياضيات'],
      gender: ['ذكر'],
      imageFile: [null, [Validators.required]]
    });
  }

  onSubmit() {
    if (this.addTeacherForm.invalid) {
      return;
    }

    const formData = new FormData();

    const formValues = this.addTeacherForm.value;
    for (const key of Object.keys(formValues)) {
      formData.append(key, formValues[key]);
    }

    this.loading = true;
    this.accountService.addTeacher(formData).subscribe({
      next: () => {
        this.toastr.success('Teacher added successfuly');
        this.loading = false;
      },
      error : err => {
        console.log(err);
        this.loading = false;
      }
    })
  }

  onReset() {
    
    this.addTeacherForm.reset({
      major: 'رياضيات',
      gender: 'ذكر'
    });
    this.fileInput.nativeElement.value = null;
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    
    if (!file) {
      this.addTeacherForm.get('imageFile').setValue(file);
      return;
    }

    if (!file.type.includes('image')) {
      this.toastr.error('only accept images!');
      this.fileInput.nativeElement.value = null;
      return;
    }

    this.addTeacherForm.get('imageFile').setValue(file);
  }
}
