import { Component, OnInit } from '@angular/core';
import { Course } from '../models/course';
import { SectionItem } from '../models/sectionItem';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { StudentService } from '../services/student.service';

@Component({
  selector: 'app-course',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})
export class CourseComponent implements OnInit{

  activeSectionItem: SectionItem = null;
  opendSection: number = null;//if last watched lecture

  constructor(private studentService: StudentService,private route: ActivatedRoute) {}
  isOpen: boolean[] = [];
  course: Course;
  
  ngOnInit(): void {
    const courseId = +this.route.params['_value'].id;
    
    this.studentService.getCourse(courseId).subscribe(result => {
      this.course = result;
      this.isOpen = Array(this.course.sections.length).fill(false)
      console.log(this.course);
      this.setLastWatched();
    })
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
      this.studentService.markLectureAsWatched(data.id).subscribe();
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
}
