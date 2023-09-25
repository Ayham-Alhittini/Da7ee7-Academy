import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-course-nav',
  templateUrl: './course-nav.component.html',
  styleUrls: ['./course-nav.component.css']
})
export class CourseNavComponent{
  @Input('subject') Subject: string;
  @Output('menuToggle') menuToggle = new EventEmitter();
  @Input('expanded')expanded: boolean = false;

  onMenuClick() {
    this.expanded = !this.expanded;
    this.menuToggle.emit();
  }
}
