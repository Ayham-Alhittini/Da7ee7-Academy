import { Component } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  constructor(public accountService: AccountService){}
  activePage = 'home';
  pages = [
    'home',
    'courses',
    'teachers',
    'blogs',
    'sell-points'
  ];
}
