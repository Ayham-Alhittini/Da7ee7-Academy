import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Blog } from 'src/app/models/Blog';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-blog-details',
  templateUrl: './blog-details.component.html',
  styleUrls: ['./blog-details.component.css']
})
export class BlogDetailsComponent implements OnInit{
  
  id: number;
  constructor(route: ActivatedRoute, private userService: UserService) {
    this.id = +route.snapshot.paramMap.get('id');
  }

  blog: Blog = null;
  ngOnInit(): void {
    this.userService.getBlog(this.id).subscribe(result => {
      this.blog = result;
      
      this.blog.content = this.blog.content.replaceAll('\n', '<br>')
    });
  }

}
