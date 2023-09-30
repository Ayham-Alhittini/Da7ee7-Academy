import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from '../services/admin.service';

@Component({
  selector: 'app-test-cards',
  templateUrl: './test-cards.component.html',
  styleUrls: ['./test-cards.component.css']
})
export class TestCardsComponent implements OnInit{
  
  cards = [];

  constructor(private userService: UserService, public accountService: AccountService, 
    private toastr: ToastrService, private adminService: AdminService) { }
  
  ngOnInit(): void {
    this.loadCards();
  }

  loadCards() {
    this.userService.getTestCards().subscribe(result => {
      this.cards = result;
    });
  }

  generateCards() {
    const response = window.prompt('Enter the number of cards to generate:');
    if (response !== null) {
      const numberOfCards = parseInt(response, 10);
      if (!numberOfCards || numberOfCards < 1) {
        this.toastr.error('Please enter a valid number', 'Error');
        return;
      }
      this.toastr.show('loading', null, {
        positionClass: 'toast-bottom-center',
        timeOut: 1000,
      });

      this.adminService.generateCards(numberOfCards).subscribe(() => {
        this.loadCards();
      });

    }
  }

}
