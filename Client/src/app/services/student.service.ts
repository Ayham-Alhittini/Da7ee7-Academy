import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Course } from '../models/course';
import { HttpClient } from '@angular/common/http';
import { Teacher } from '../models/teacher';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private baseUrl = environment.apiBaseUrl + 'student/';
  constructor(private http: HttpClient) { }

  getCourse(id: number){
    return this.http.get<Course>(this.baseUrl + 'course/' + id);
  }

  markLectureAsWatched(id: number) {
    return this.http.put(this.baseUrl + 'mark-watched/' + id, null);
  }

  enrollInCourse(enrolledCourse: any) {
    return this.http.post(this.baseUrl + 'enroll-in-course', enrolledCourse);
  }

  getMyCourses() {
    return this.http.get<{courses: Course[], teachers: Teacher[]}>(this.baseUrl + 'my-courses');
  }


  getScientificSubjects() {
    return ["رياضيات", "فيزياء", "كيمياء", "احياء"];
  }

  getLiterarySubjects() {
    return [
      "رياضيات",
     "عربي تخصص",
       "الثقافة المالية",
       "الحاسوب", 
       "جغرافيا",
       "الدراسات الاسلامية"
     ];
  }

  getCommonSubjects() {
    return [
      "اللغة الانجليزية",
      "مهارات الاتصال",
      "التربية الاسلامية",
      "تاريخ الاردن"
    ]
  }

  getSubjects() {
    return [...this.getScientificSubjects(), ...this.getLiterarySubjects(), ...this.getCommonSubjects()];
  }
}
