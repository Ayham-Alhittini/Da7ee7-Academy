import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Course } from 'src/app/models/course';
import { Teacher } from 'src/app/models/teacher';
import { StudentService } from 'src/app/services/student.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-enroll-in-course',
  templateUrl: './enroll-in-course.component.html',
  styleUrls: ['./enroll-in-course.component.css']
})
export class EnrollInCourseComponent {
  @Input('course') course: Course;
  @Input('teacher') teacher: Teacher;

  cardNumber: string = '';
  submitted = false;

  error: string;

  constructor(public userService: UserService, private studentService: StudentService, private router: Router) { }

  onSubmit() {
    this.submitted = true;
    if (this.cardNumber === '') {
      return;
    }

    this.studentService.enrollInCourse({courseId: this.course.id, cardNumber: this.cardNumber})
    .subscribe(response => {
      this.error = response['error'];
      if (!this.error) {
        location.reload();
      }
    });

  }
}
