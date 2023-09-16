import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Course } from '../models/course';
import { HttpClient } from '@angular/common/http';

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
}
