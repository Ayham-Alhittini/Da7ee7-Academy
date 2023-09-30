import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './auth/login/login.component';
import { IsGuestGuard } from './guards/is-guest.guard';
import { SingupComponent } from './auth/singup/singup.component';
import { AdminComponent } from './admin/admin.component';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { CoursesComponent } from './admin/courses/courses.component';
import { TeachersComponent } from './admin/teachers/teachers.component';
import { StudentsComponent } from './admin/students/students.component';
import { CoursesManagementComponent } from './admin/courses-management/courses-management.component';
import { SectionComponent } from './admin/courses-management/section/section.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { CoursesViewComponent } from './courses-view/courses-view.component';
import { TeachersViewComponent } from './teachers-view/teachers-view.component';
import { SalePointsViewComponent } from './sale-points-view/sale-points-view.component';
import { BlogsViewComponent } from './blogs-view/blogs-view.component';
import { CoursesListComponent } from './courses-view/courses-list/courses-list.component';
import { TeacherProfileComponent } from './teachers-view/teacher-profile/teacher-profile.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { BlogDetailsComponent } from './blogs-view/blog-details/blog-details.component';
import { ForbiddenComponent } from './errors/forbidden/forbidden.component';
import { IsAdminGuard } from './guards/is-admin.guard';
import { CourseComponent } from './course/course.component';
import { AuthGuard } from './guards/auth.guard';
import { MyCoursesComponent } from './courses-view/my-courses/my-courses.component';
import { SettingsComponent } from './settings/settings.component';
import { PersonalInformationComponent } from './settings/personal-information/personal-information.component';
import { PrivacyComponent } from './settings/privacy/privacy.component';
import { TestCardsComponent } from './test-cards/test-cards.component';

const routes: Routes = [

  // unprotected routes
  {path : '', component : HomeComponent, pathMatch: 'full'},
  {path: 'courses', component: CoursesViewComponent},///to show course sections
  {path: 'courses/:major', component: CoursesListComponent},
  {path: 'teachers', component: TeachersViewComponent},
  {path: 'teachers/:id', component: TeacherProfileComponent},
  {path: 'blogs', component: BlogsViewComponent},
  {path: 'blogs/:id', component: BlogDetailsComponent},
  {path: 'sale-points', component: SalePointsViewComponent},
  {path: 'test-cards', component: TestCardsComponent},

  /// authinticated routes
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      /// if admin let him in, if student let him in in case he is enrolled in the course, otherwise redirect to enrollment page
      {path: 'course/:id', component: CourseComponent},
      {path: 'my-courses', component: MyCoursesComponent},
    ]
  },


  /// settings routes
  {
    path: 'settings',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    component: SettingsComponent,
    children: [
      {path: '', component: PersonalInformationComponent, pathMatch: 'full'},
      {path: 'privacy', component: PrivacyComponent},
    ]
  },


  // authontication routes
  {
    path : '',
    runGuardsAndResolvers: 'always',
    canActivate: [IsGuestGuard],
    children: [
      {path : 'login', component: LoginComponent},
      {path : 'singup', component: SingupComponent},
    ]
  },

  // admin routes
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [IsAdminGuard],
    children: [
      {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
      {path: 'dashboard', component: DashboardComponent},
      {path: 'teachers', component: TeachersComponent},
      {path: 'students', component: StudentsComponent},
      {path: 'courses', component: CoursesComponent},///for admin to manage the courses
    ]
  },

  // courses management routes
  {
    path: 'courses-management',
    runGuardsAndResolvers: 'always',
    canActivate: [IsAdminGuard],
    children: [
      {path: 'course/:id', component: CoursesManagementComponent},
      {path: 'section/:id', component: SectionComponent},
    ]
  },

  ///errors routes
  {path: 'not-found', component: NotFoundComponent},
  {path: 'foribidden', component: ForbiddenComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }