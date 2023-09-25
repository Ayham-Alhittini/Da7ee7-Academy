import { Component, OnInit } from '@angular/core';
import { Teacher } from '../models/teacher';
import { UserService } from '../services/user.service';
import { StudentService } from '../services/student.service';

@Component({
  selector: 'app-teachers-view',
  templateUrl: './teachers-view.component.html',
  styleUrls: ['./teachers-view.component.css']
})

export class TeachersViewComponent implements OnInit {

  teachersSrc: Teacher[] = [];
  teachers: Teacher[] = [];
  constructor(public userService: UserService, public studentService: StudentService) {}

  ngOnInit(): void {
    this.loadTeachers();
  }

  loadTeachers() {
    this.userService.getTeachers().subscribe({
      next: result => {
        this.teachersSrc = result;
        this.teachers = this.teachersSrc.slice();
      }
    })
  }

  onSubjectChange(event: any) {
    const newSubject = event.target.value;
    this.setTeachersBySubject(newSubject);
  }

  setTeachersBySubject(newSubject: string) {
    this.teachers = this.teachersSrc.slice();

    if (newSubject !== 'all') {
      this.teachers = this.teachersSrc.filter(teacher => teacher.major === newSubject);
    }
  }

}
