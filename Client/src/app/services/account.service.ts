import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {BehaviorSubject, tap} from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http : HttpClient) { }

  private baseUrl = environment.apiBaseUrl;

  loadedUser = new BehaviorSubject<User>(null);

  login(model : any)
  {
    return this.http.post<User>(this.baseUrl + "account/login", model).pipe(
      tap(
        res => {
          this.setCurrentUser(res);
        },
      )
    );
  }


  autoLogin()
  {
    const user = localStorage.getItem('user');

    if (user) {
      this.loadedUser.next(JSON.parse(user));
    }
  }

  logout() {
    localStorage.removeItem('user');
    this.loadedUser.next(null);
    window.location.href = '/';
  }

  studentRegister(model : any) {
    return this.http.post<User>(this.baseUrl + "account/student-register", model);
  }

  getProfile() {
    return this.http.get<User>(this.baseUrl + "account/get-profile");
  }

  updateProfile(model : any) {
    return this.http.post(this.baseUrl + "account/update-profile", model);
  }

  chanePassword(modal: any) {
    return this.http.post(this.baseUrl + "account/change-password", modal);
  }

  setCurrentUser(user : User) {
    this.loadedUser.next(user);
    localStorage.setItem('user', JSON.stringify(user));
  }

  changePassword(model: any) {
    return this.http.post(this.baseUrl + "account/change-password", model);
  }

  addTeacher(newTeacher: any) {
    return this.http.post(this.baseUrl + "account/teacher-register", newTeacher);
  }
}