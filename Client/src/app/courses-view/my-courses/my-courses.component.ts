import { Component, OnInit } from '@angular/core';
import { Course } from 'src/app/models/course';
import { Member } from 'src/app/models/member';
import { Teacher } from 'src/app/models/teacher';
import { StudentService } from 'src/app/services/student.service';

@Component({
  selector: 'app-my-courses',
  templateUrl: './my-courses.component.html',
  styleUrls: ['./my-courses.component.css']
})
export class MyCoursesComponent implements OnInit {

  courses: Course[] = [];
  teachers: Teacher[] = [];
  constructor(private studentService: StudentService) { }

  ngOnInit(): void {
    this.studentService.getMyCourses().subscribe(res => {
      console.log(res);
      this.courses = res.courses;
      this.teachers = res.teachers;
    });
  }

  getTeacher(teacherId: string) : Member{
    return this.teachers.find(t => t.id === teacherId)?.user;
  }
} 
