import { Component, OnInit } from '@angular/core';
import { Course } from '../models/course';
import { SectionItem } from '../models/sectionItem';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { StudentService } from '../services/student.service';
import { AccountService } from '../services/account.service';
import { UserService } from '../services/user.service';
import { Teacher } from '../models/teacher';

@Component({
  selector: 'app-course',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})
export class CourseComponent implements OnInit{

  courseId = null;
  course: Course;
  isEnrolled: boolean = null;
  courseTeacher: Teacher = null;

  constructor(private studentService: StudentService, 
    private userService: UserService,
    private route: ActivatedRoute) {
    const courseId = +this.route.params['_value'].id;
    this.courseId = courseId;
  }

  ngOnInit(): void {
    this.loadCourse();
  }

  loadCourse() {
    this.studentService.getCourse(this.courseId).subscribe(result => {
      this.course = result;

      
      if (result.isEnrolled) {
        this.isEnrolled = true;
      } else {
        ///get the teacher that teach this course
        const teacherId = result.teacherId;
        this.userService.getSingleTeacher(teacherId).subscribe(teacher => {
          this.courseTeacher = teacher;
          this.isEnrolled = false;
        });
      }
    })
  }
}
