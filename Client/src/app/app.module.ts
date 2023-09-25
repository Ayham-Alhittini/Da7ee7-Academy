import { NgModule } from "@angular/core";
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NavbarComponent } from './navbar/navbar.component';
import { SharedModule } from './modules/shared.modules';
import { ReactiveFormsModule } from '@angular/forms';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FloorPipe } from './Pipes/floor.pipe';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { HomeComponent } from './home/home.component';
import { ManageCoursesComponent } from './manage-courses/manage-courses.component';
import { LoginComponent } from './auth/login/login.component';
import { SingupComponent } from './auth/singup/singup.component';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { AdminComponent } from './admin/admin.component';
import { SideBarComponent } from './admin/side-bar/side-bar.component';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { CoursesComponent } from './admin/courses/courses.component';
import { TeachersComponent } from './admin/teachers/teachers.component';
import { StudentsComponent } from './admin/students/students.component';
import { CourseComponent } from './course/course.component';
import { CourseNavComponent } from './course/course-nav/course-nav.component';
import { CourseVideoComponent } from './course/course-video/course-video.component';
import { CoursesManagementComponent } from './admin/courses-management/courses-management.component';
import { SectionComponent } from './admin/courses-management/section/section.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { CoursesViewComponent } from './courses-view/courses-view.component';
import { TeachersViewComponent } from './teachers-view/teachers-view.component';
import { BlogsViewComponent } from './blogs-view/blogs-view.component';
import { SalePointsViewComponent } from './sale-points-view/sale-points-view.component';
import { CoursesListComponent } from './courses-view/courses-list/courses-list.component';
import { TeacherProfileComponent } from './teachers-view/teacher-profile/teacher-profile.component';
import { BlogDetailsComponent } from './blogs-view/blog-details/blog-details.component';
import { ForbiddenComponent } from './errors/forbidden/forbidden.component';
import { EnrollInCourseComponent } from './course/enroll-in-course/enroll-in-course.component';
import { CourseContentComponent } from './course/course-content/course-content.component';
import { MyCoursesComponent } from './courses-view/my-courses/my-courses.component';
import { SettingsComponent } from './settings/settings.component';
import { PersonalInformationComponent } from './settings/personal-information/personal-information.component';
import { PrivacyComponent } from './settings/privacy/privacy.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FloorPipe,
    ServerErrorComponent,
    HomeComponent,
    ManageCoursesComponent,
    LoginComponent,
    SingupComponent,
    AdminComponent,
    SideBarComponent,
    DashboardComponent,
    CoursesComponent,
    TeachersComponent,
    StudentsComponent,
    CourseComponent,
    CourseNavComponent,
    CourseVideoComponent,
    CoursesManagementComponent,
    SectionComponent,
    NotFoundComponent,
    CoursesViewComponent,
    TeachersViewComponent,
    BlogsViewComponent,
    SalePointsViewComponent,
    CoursesListComponent,
    TeacherProfileComponent,
    BlogDetailsComponent,
    ForbiddenComponent,
    EnrollInCourseComponent,
    CourseContentComponent,
    MyCoursesComponent,
    SettingsComponent,
    PersonalInformationComponent,
    PrivacyComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    {provide : HTTP_INTERCEPTORS, useClass : JwtInterceptor, multi : true},
    {provide : HTTP_INTERCEPTORS, useClass : ErrorInterceptor, multi : true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
