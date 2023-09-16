import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ManageCoursesService } from 'src/app/services/manage-courses.service';

@Component({
  selector: 'app-courses-management',
  templateUrl: './courses-management.component.html',
  styleUrls: ['./courses-management.component.css']
})
export class CoursesManagementComponent implements OnInit{

  constructor(private route: ActivatedRoute, 
    private manageCourseService: ManageCoursesService) {}
  courseId = null;
  sections: {id: number, sectionTitle: string, orderNumber: number}[]  = [];
  _newSection = null;
  ngOnInit(): void {
    const courseId = +this.route.params['_value'].id;
    this.courseId = courseId;
    this.getSections(courseId);
  }

  private getSections(courseId: number) {
    this.manageCourseService.getCourseSections(courseId).subscribe(result => {
      this.sections = result;
    });
  }

  newSection() {
    if (this._newSection && this.courseId) {
      this.manageCourseService
      .addCourseSection({courseId: this.courseId, sectionTitle: this._newSection})
      .subscribe(() => {
        this.getSections(this.courseId);
      })
    }
    this._newSection = null;
  }
}
