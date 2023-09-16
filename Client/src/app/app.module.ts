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
import { ModeratorsComponent } from './admin/moderators/moderators.component';
import { TeachersComponent } from './admin/teachers/teachers.component';
import { StudentsComponent } from './admin/students/students.component';
import { BlogsComponent } from './admin/blogs/blogs.component';
import { SalePointsComponent } from './admin/sale-points/sale-points.component';
import { CourseComponent } from './course/course.component';
import { CourseNavComponent } from './course/course-nav/course-nav.component';
import { CourseVideoComponent } from './course/course-video/course-video.component';
import { CoursesManagementComponent } from './admin/courses-management/courses-management.component';
import { SectionComponent } from './admin/courses-management/section/section.component';

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
    ModeratorsComponent,
    TeachersComponent,
    StudentsComponent,
    BlogsComponent,
    SalePointsComponent,
    CourseComponent,
    CourseNavComponent,
    CourseVideoComponent,
    CoursesManagementComponent,
    SectionComponent
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
