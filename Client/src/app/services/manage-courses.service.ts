import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Section } from '../models/section';

@Injectable({
  providedIn: 'root'
})
export class ManageCoursesService {

  private baseUrl = environment.apiBaseUrl + 'CourseManagement';
  constructor(private http: HttpClient) { }

  addCourse(newCourse: any) {
    return this.http.post(this.baseUrl + '/add-course', newCourse);
  }
  addCourseSection(newSection: any) {
    return this.http.post(this.baseUrl + '/add-course-section', newSection);
  }

  addCourseSectionItem(newSectionItem: any) {
    return this.http.post(this.baseUrl + '/add-section-item', newSectionItem);
  }
  getCourseSections(courseId: number) {
    return this.http
    .get<{id: number, sectionTitle: string, orderNumber: number}[]>
    (this.baseUrl + '/get-course-sections/' + courseId);
  }

  getCourseSection(sectionId: number) {
    return this.http
    .get<Section>
    (this.baseUrl + '/get-course-section/' + sectionId);
  }

  deleteCourse(courseId: number) {
    return this.http.delete(this.baseUrl + '/delete-course/' + courseId);
  }

  deleteCourseSection(sectionId: number) {
    return this.http.delete(this.baseUrl + '/delete-course-section/' + sectionId);
  }

  editSectionTitle(sectionId: number, newSectionTitle: string) {
    return this.http
    .patch(this.baseUrl + '/update-section-title/' + sectionId + '?NewSectionTitle='+ newSectionTitle 
    , null);
  }

  editSectionItem(formData: any) {
    return this.http.patch(this.baseUrl + '/update-section-item', formData);
  }

  editSectionItemsOrder(newOrder: any) {
    return this.http.patch(this.baseUrl + '/edit-section-items-order', newOrder);
  }

  editSectionsOrder(newOrder: any) {
    return this.http.patch(this.baseUrl + '/edit-sections-order', newOrder);
  }
}
