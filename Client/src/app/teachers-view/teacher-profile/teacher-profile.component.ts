import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Course } from 'src/app/models/course';
import { Teacher } from 'src/app/models/teacher';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-teacher-profile',
  templateUrl: './teacher-profile.component.html',
  styleUrls: ['./teacher-profile.component.css']
})
export class TeacherProfileComponent implements OnInit {
  id: string;
  teacherProfile: {teacher: Teacher,courses: Course[]} = null;

  constructor(route: ActivatedRoute, public userService: UserService) {
    this.id = route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    this.loadTeacherProfile();
  }

  loadTeacherProfile() {
    this.userService.getTeacherProfile(this.id).subscribe(result => {
      this.teacherProfile = result;
    })
  }
}
