import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Course } from '../models/course';
import { Teacher } from '../models/teacher';
import { Blog } from '../models/Blog';
import { Member } from '../models/member';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{

  homeData : {courses: Course[], teachers: Teacher[], blogs: Blog[]} = null;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getHome().subscribe(result => {
      this.homeData = result;
    })
  }

  getTeacher(teacherId: string) : Member{
    return this.homeData.teachers.find(t => t.id === teacherId)?.user;
  }
}
