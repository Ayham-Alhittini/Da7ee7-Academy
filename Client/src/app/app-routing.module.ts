import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './auth/login/login.component';
import { IsGuestGuard } from './guards/is-guest.guard';
import { SingupComponent } from './auth/singup/singup.component';
import { AdminComponent } from './admin/admin.component';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { CoursesComponent } from './admin/courses/courses.component';
import { ModeratorsComponent } from './admin/moderators/moderators.component';
import { TeachersComponent } from './admin/teachers/teachers.component';
import { StudentsComponent } from './admin/students/students.component';
import { BlogsComponent } from './admin/blogs/blogs.component';
import { SalePointsComponent } from './admin/sale-points/sale-points.component';
import { CourseComponent } from './course/course.component';
import { CoursesManagementComponent } from './admin/courses-management/courses-management.component';
import { SectionComponent } from './admin/courses-management/section/section.component';

const routes: Routes = [
  {path : '', component : HomeComponent},
  {path: 'course/:id', component: CourseComponent},
  {path : 'home', component : HomeComponent},
  {
    path : '',
    runGuardsAndResolvers: 'always',
    canActivate: [IsGuestGuard],
    children: [
      {path : 'login', component: LoginComponent},
    {path : 'singup', component: SingupComponent},
    ]
  },
  {
    path: 'admin',
    component: AdminComponent,
    children: [
      {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
      {path: 'dashboard', component: DashboardComponent},
      {path: 'moderators', component: ModeratorsComponent},
      {path: 'teachers', component: TeachersComponent},
      {path: 'students', component: StudentsComponent},
      {path: 'courses', component: CoursesComponent},
      {path: 'blogs', component: BlogsComponent},
      {path: 'sale-points', component: SalePointsComponent},
    ]
  },
  //it also for admin but it's have diffent page style
  {path: 'courses-management/course/:id', component: CoursesManagementComponent},
  {path: 'courses-management/section/:id', component: SectionComponent},

  ///replacment of 404 page currently
  {path: '**', redirectTo: 'home'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
