import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-course-nav',
  templateUrl: './course-nav.component.html',
  styleUrls: ['./course-nav.component.css']
})
export class CourseNavComponent {
  @Input('subject') Subject: string;
}
