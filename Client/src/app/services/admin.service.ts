import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private baseUrl = environment.apiBaseUrl + 'admin';
  constructor(private http: HttpClient) { }

  getTeachers(){
    return this.http.get<{id: string, fullName: string}[]>(this.baseUrl + '/get-teachers');
  }

  getCourses() {
    return this.http.get(this.baseUrl + '/get-courses');
  }
}
