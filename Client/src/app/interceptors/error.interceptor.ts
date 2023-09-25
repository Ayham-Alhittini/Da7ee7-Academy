import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router : Router, private toastr : ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error : HttpErrorResponse) => {
        const errorMsg = error.error;
        if (error) {
          switch(error.status)
          {
            case 400 :
              if (error.error.errors) {
                const modelStateErrors = [];
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                throw modelStateErrors.flat();
              } else {
                this.toastr.error(error.error, error.status.toString());
              }
              break;
              case 401 :
                this.router.navigateByUrl("/login");
                if (error.error) {
                  this.toastr.error(error.error, null, {
                    positionClass: 'toast-bottom-center'
                  });
                }
                break;
              case 404 :
                this.toastr.error(errorMsg, "404");
                break;
              case 500 :
                this.router.navigateByUrl("/server-error");
                break;
              default :
                this.toastr.error("Something Unexpected went wrong");
                console.log(error);
                break;
          }
        }
        throw error;
      })
    );
  }
}
