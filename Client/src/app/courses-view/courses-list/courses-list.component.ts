import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Course } from 'src/app/models/course';
import { Member } from 'src/app/models/member';
import { Teacher } from 'src/app/models/teacher';
import { StudentService } from 'src/app/services/student.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-courses-list',
  templateUrl: './courses-list.component.html',
  styleUrls: ['./courses-list.component.css']
})
export class CoursesListComponent implements OnInit{

  major: string;
  subjects: string[] = [];
  teachers: Teacher[] = [];
  courses: Course[] = [];

  activeSubject = 'none';
  activeTeacher = 'none';

  teachersSrc: Teacher[] = [];
  coursesSrc: Course[] = [];
  loading = false;

  constructor(private route: ActivatedRoute, 
    private router: Router, 
    private userService: UserService, 
    private studentService: StudentService){}

  ngOnInit(): void {
    this.loading = true;
    this.getMajorFromRoute(); 
    this.loadTeachers();
    this.loadCourses();
    ///finsh loading when load courses finshed
  }
  
  getMajorFromRoute() {
    this.major = this.route.snapshot.paramMap.get('major');

    switch(this.major)
    {
      case 'scientific':
        this.major = 'علمي';
        this.subjects = this.studentService.getScientificSubjects();
        break;
      case 'literary':
        this.major = 'ادبي';
        this.subjects = this.studentService.getLiterarySubjects();
        break;
      case 'common-subjects':
        this.major = 'مشترك';
        this.subjects = this.studentService.getCommonSubjects();
        break;
      default: 
        this.router.navigateByUrl("/not-found");
    }

  }

  loadTeachers() {
    this.userService.getTeachers().subscribe({
      next: result => {
        this.teachersSrc = result;
      }
    })
  }

  loadCourses() {
    this.userService.getCourses(this.major).subscribe({
      next: result => {
        this.coursesSrc = result;
        this.courses = this.coursesSrc.slice();
        this.loading = false;
      }
    });
  }

  onSubjectChange(event: any) {
    const selectedSubject = event.target.value;
    
    this.activeSubject = selectedSubject;

    this.setTeachers(selectedSubject);
    this.setCourses();
  }

  onTeacherChange(event: any) {
    const selectedTeacher = event.target.value;

    this.activeTeacher = selectedTeacher;
    this.setCourses();
  }

  setTeachers(newSubject: string) {
    this.activeTeacher = 'none';
    if (newSubject === 'none') {
      this.teachers = [];
    } else {
      this.teachers = this.teachersSrc.filter(teacher => teacher.major === newSubject);
    }
  }

  setCourses() {
    var newCourses = this.coursesSrc.slice();

    if (this.activeSubject !== 'none') {
      newCourses = newCourses.filter(c => c.subject === this.activeSubject);
    }
    
    if (this.activeTeacher !== 'none') {
      newCourses = newCourses.filter(c => c.teacherId === this.activeTeacher);
    }
    
    this.courses = newCourses.slice();
  }

  getTeacher(teacherId: string) : Member{
    return this.teachersSrc.find(t => t.id === teacherId)?.user;
  }


}
