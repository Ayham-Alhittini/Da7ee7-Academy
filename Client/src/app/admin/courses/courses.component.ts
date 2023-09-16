import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/services/admin.service';
import { ManageCoursesService } from 'src/app/services/manage-courses.service';
@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.css']
})
export class CoursesComponent implements OnInit{

  loading = false;
  submitted = false;
  invalidUploadFile = false;
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
  teachers: {id: string, fullName: string}[] = [];
  courses: any = [];
  
  constructor(private adminService: AdminService, 
    private manageCoursesService: ManageCoursesService, 
    private toastrService: ToastrService){}

  ngOnInit(): void {
    this.loading = true;
    this.loadTeachers();
    this.loadCourses();
    this.loading = false;
  }

  private loadTeachers() {
    this.adminService.getTeachers().subscribe(result => {
      this.teachers = result;
      if (this.teachers.length > 0) {
        this.modelData.teacherId = this.teachers[0].id;
      }
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
    if (this.modelData.courseCover === null) {
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
      console.log('added successfuly');
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

  onFileSelected(event: any): void {
    this.modelData.courseCover = event.target.files[0];
  }

  onMajorChange(event: any) {
    const newMajor = event.target.value;
    this.modelData.major = newMajor;
    this.modelData.subject = this.subjects[newMajor][0];
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
