import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Teacher } from '../models/teacher';
import { Course } from '../models/course';
import { SalePoint } from '../models/sale-point';
import { Blog } from '../models/Blog';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl = environment.apiBaseUrl + 'user';
  constructor(private http: HttpClient) { }

  getTeachers(){
    return this.http.get<Teacher[]>(this.baseUrl + '/get-teachers');
  }

  getSingleTeacher(id: string) {
    return this.http.get<Teacher>(this.baseUrl + '/get-teacher/' + id);
  }

  getCourses(major: string) {
    return this.http.get<Course[]>(this.baseUrl + '/get-courses/' + major);
  }

  getTeacherProfile(teacherId: string) {
    return this.http.get<{teacher: Teacher, courses: Course[]}>
      (this.baseUrl + '/get-teacher-profile/' + teacherId);
  }


  getSalePoints() {
    return this.http.get<SalePoint[]>(this.baseUrl + '/get-sale-points');
  }

  getBlogs() {
    return this.http.get<Blog[]>(this.baseUrl + '/get-blogs');
  }

  getBlog(id: number) {
    return this.http.get<Blog>(this.baseUrl + '/get-blog/' + id);
  }


  getHome() {
    return this.http.get<{courses: Course[], teachers: Teacher[], blogs: Blog[]}>(this.baseUrl + '/get-home');
  }

  ////Not http methods
  getFirstAndLastName(fullName: string) {
    
    const nameParts = fullName.split(' ');

    if (nameParts.length <= 2) {
      return fullName
    }
    return nameParts[0] + ' ' + nameParts[nameParts.length - 1];
  }
  
  getGovernorates() {
    return [
      "عمان",
      "الزرقاء",
      "اربد",
      "جرش",
      "البلقاء",
      "الطفيلة",
      "معان",
      "الكرك",
      "العقبة",
      "عجلون",
      "مادبا",
      "المفرق"
    ];
  }
}
