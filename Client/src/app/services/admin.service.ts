import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private baseUrl = environment.apiBaseUrl + 'admin';
  constructor(private http: HttpClient) { }

  getCourses() {
    return this.http.get(this.baseUrl + '/get-courses');
  }

  getTeachers() {
    return this.http.get(this.baseUrl + '/get-teachers');
  }

  addSalePoint(newSalePoint: any) {
    return this.http.post(this.baseUrl + '/add-sale-point', newSalePoint);
  }

  deleteSalePoint(id: number) {
    return this.http.delete(this.baseUrl + '/delete-sale-point/' + id);
  }

  addBlog(newBlog: any) {
    return this.http.post(this.baseUrl + '/add-blog' , newBlog);
  }

  deleteBlog(id: number) {
    return this.http.delete(this.baseUrl + '/delete-blog/' + id);
  }

  generateCards(numberOfCards: number) {
    return this.http.post(this.baseUrl + '/generate-cards/' + numberOfCards, null);
  }
}
