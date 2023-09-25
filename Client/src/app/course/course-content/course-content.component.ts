import { Component, Input, OnInit } from '@angular/core';
import { Course } from 'src/app/models/course';
import { SectionItem } from 'src/app/models/sectionItem';
import { AccountService } from 'src/app/services/account.service';
import { StudentService } from 'src/app/services/student.service';

@Component({
  selector: 'app-course-content',
  templateUrl: './course-content.component.html',
  styleUrls: ['./course-content.component.css']
})
export class CourseContentComponent implements OnInit {

  @Input('course') course: Course;
  activeSectionItem: SectionItem = null;
  opendSection: number = null;//if last watched lecture
  isOpen: boolean[] = [];
  isExpanded: boolean = false;///if menu is expanded for tablet and mobile view

  constructor(private accountService: AccountService, private studentService: StudentService) { }

  ngOnInit(): void {
    this.isOpen = Array(this.course.sections.length).fill(false)
    this.setLastWatched();
  }

  toggleAccordion(event: any, index: number) {
    this.isOpen[index] = event;
  }

  getTime(timeInSeconds: number): string {
    const hours = Math.floor(timeInSeconds / 3600);
    timeInSeconds %= 3600;

    const mintues = Math.floor(timeInSeconds / 60);
    timeInSeconds %= 60;

    const seconds = timeInSeconds;


    let result = '';

    if (hours > 0) {
      result += (hours < 10 ? '0' : '') + hours + ':';
    }

    result += (mintues < 10 ? '0' : '') + mintues + ':';

    result += (seconds < 10 ? '0' : '') + seconds;
    return result;
  }

  sectionItemClick(data: SectionItem) {
    if (data?.type == 2) {
      this.activeSectionItem = data;
      this.activeSectionItem.watchedDate = new Date();

      this.accountService.loadedUser.subscribe(user => { 
        if (user.role === 'Student') {
          this.studentService.markLectureAsWatched(data.id).subscribe();
        }
      });
      this.isExpanded = false;
    }
  }

  setLastWatched() {
    let lastWatched = null;
    let _opendSection = null;
    this.course.sections.forEach(section => {
      section.sectionItems.forEach(sectionItem => {
        if (sectionItem.watchedDate !== null) {
          if (lastWatched === null) {
            lastWatched = sectionItem;
            _opendSection = section.id;
          } else {
            if (sectionItem.watchedDate > lastWatched.watchedDate) {
              lastWatched = sectionItem;
              _opendSection = section.id;
            }
          }
        }
      });
    });

    if (lastWatched) {
      this.activeSectionItem = lastWatched;
      this.opendSection = _opendSection;
    }
  }

  menuToggle() {
    this.isExpanded = !this.isExpanded;
  }
}
