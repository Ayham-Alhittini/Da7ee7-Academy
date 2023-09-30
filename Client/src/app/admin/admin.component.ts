import { Component } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {
  activeChild: string;
  constructor(router: Router) {
    router.events.subscribe({
      next: (event: any) => {
        if(event instanceof NavigationEnd){
          const url = event.url;
          if (url) {
            this.activeChild = this.getChild(url);
          }
        }
      }
    })
  }

  getChild(url: string) {
    let startIndex = url.lastIndexOf('/') + 1;
    let child = url.substring(startIndex);
     return child.at(0).toUpperCase() + child.substring(1);
  }
}
