import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Teacher } from 'src/app/models/teacher';
import { AdminService } from 'src/app/services/admin.service';
import { ManageCoursesService } from 'src/app/services/manage-courses.service';
import { UserService } from 'src/app/services/user.service';
@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.css']
})
export class CoursesComponent implements OnInit{

  loading = false;
  submitted = false;
  invalidUploadFile = false;
  onlyNull = null;
  teachersSrc: Teacher[] = [];
  modelData = {
    subject: 'رياضيات',
    teacherId: '',
    major: 'علمي',
    courseCover: null
  }
  subjects = {
    علمي : ["رياضيات", "فيزياء", "كيمياء", "احياء", "علوم ارض"],
    ادبي : ["رياضيات", "عربي تخصص", "الحاسوب", "الجغرافيا", "الدراسات الاسلامية"],
    مشترك: ["اللغة الانجليزية", "مهارات الاتصال", "التربية الاسلامية", "تاريخ الاردن"]
  }
  teachers: Teacher[] = [];
  courses: any = [];
  
  constructor(private adminService: AdminService, 
    private userService: UserService,
    private manageCoursesService: ManageCoursesService, 
    private toastrService: ToastrService){}

  ngOnInit(): void {
    this.loading = true;
    this.loadCourses();
    this.loadTeachers();
    this.loading = false;
  }

  private loadTeachers() {
    this.userService.getTeachers().subscribe(result => {
      this.teachersSrc = result;
      this.setTeachers();
    });
  }

  private loadCourses() {
    this.adminService.getCourses().subscribe(result => {
      this.courses = result;
    })
  }

  onSubmit() {
    this.submitted = true;
    
    this.invalidUploadFile = false;
    if (this.modelData.courseCover === null || !this.teachers.length) {
      return;
    }

    if (!this.isImage(this.modelData.courseCover)) {
      this.invalidUploadFile = true;
      return;
    }
    const formData = new FormData();

    formData.append('subject', this.modelData.subject);
    formData.append('teacherId', this.modelData.teacherId);
    formData.append('major', this.modelData.major);
    formData.append('courseCover', this.modelData.courseCover);
    //send the course to api
    this.manageCoursesService.addCourse(formData).subscribe(() => {
      this.toastrService.success("Course added successfuly", null, {
        positionClass: 'toast-bottom-center'
      });
      this.loadCourses();
      this.ResetModal();
    });
  }

  getTime(timeInSeconds: number): string {
    const hours = Math.floor(timeInSeconds / 3600);
    timeInSeconds %= 3600;

    const mintues = Math.floor(timeInSeconds / 60);
    timeInSeconds %= 60;

    const seconds = timeInSeconds;


    let result = '';

    if (hours > 0) {
      result += (hours < 10 ? '0' : '') + hours + ':';
    }

    result += (mintues < 10 ? '0' : '') + mintues + ':';

    result += (seconds < 10 ? '0' : '') + seconds;
    return result;
  }

  onDeleteClick(courseId: number) {
    const response = confirm("Are you sure you want to delete the course");
    if (response) {
      this.manageCoursesService.deleteCourse(courseId).subscribe({
        next: () => {
          this.toastrService.warning("Course deleted");
          this.loadCourses();
        }
      })
    }
  }

  ResetModal() {
    this.modelData = {
      subject: 'رياضيات',
      teacherId: '',
      major: 'علمي',
      courseCover: null
    }
    this.submitted = false;
    this.invalidUploadFile = false;
    this.setTeachers();
    this.onlyNull = null;
  }

  onFileSelected(event: any): void {
    this.modelData.courseCover = event.target.files[0];
  }


  setTeachers() {
    if (this.teachersSrc.length && this.modelData.subject) {
      this.teachers = this.teachersSrc.filter(t => t.major === this.modelData.subject);
      if (this.teachers.length) {
        this.modelData.teacherId = this.teachers[0].id;
      }
    }
  }

  onMajorChange(event: any) {
    const newMajor = event.target.value;
    this.modelData.major = newMajor;
    this.modelData.subject = this.subjects[newMajor][0];
    this.setTeachers();
  }

  onSubjectChange(event: any) {
    const newSubject = event.target.value;
    this.modelData.subject = newSubject;
    this.setTeachers();
  }

  private isImage(file: File): boolean {
    
    const fileName = file?.name;
    
    return (
      fileName.endsWith('.jpg') ||
      fileName.endsWith('.jpeg') ||
      fileName.endsWith('.png') ||
      fileName.endsWith('.gif') ||
      fileName.endsWith('.bmp') || 
      fileName.endsWith('.jfif')
      // Add more image extensions if needed
    );
  }
}
