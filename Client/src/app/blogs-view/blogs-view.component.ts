import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Blog } from '../models/Blog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from '../services/admin.service';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-blogs-view',
  templateUrl: './blogs-view.component.html',
  styleUrls: ['./blogs-view.component.css']
})
export class BlogsViewComponent implements OnInit {
  
  addBlogForm: FormGroup;
  constructor(private userService: UserService, 
    public accountService: AccountService,
    private adminService: AdminService,
    private fb: FormBuilder, 
    private toastr: ToastrService){}

  blogsSrc: Blog[] = [];
  blogs: Blog[] = [];
  
  fileValue = null;
  invalidFile = false;

  searchText = '';

  ngOnInit(): void {
    this.loadBlogs();
    this.initializeForm();
  }

  initializeForm() {
    this.addBlogForm = this.fb.group({
      title: ['', Validators.required],
      blogPhoto: [null, Validators.required],
      content: ['', Validators.required]
    });
  }

  onBlogPhotoSelected(event: any) {
    const file = event.target.files[0];
    this.invalidFile = false;
    if (!file) {
      this.fileValue = null;
      this.invalidFile = true;
      this.addBlogForm.get('blogPhoto').setValue(null);
      return;
    }
    if (!this.isImage(file)) {
      this.toastr.error("only accept images", null, {
        positionClass: 'toast-bottom-center'
      });
      this.fileValue = null;
      this.invalidFile = true;
      this.addBlogForm.get('blogPhoto').setValue(null);
      return;
    }
    this.addBlogForm.get('blogPhoto').setValue(file);
  }

  onAddBlogSubmit() {
    if (this.addBlogForm.invalid) {
      return;
    }

    const formData = new FormData();

    const formValues = this.addBlogForm.value;

    for (const key of Object.keys(formValues)) {
      formData.append(key, formValues[key]);
    }
    
    this.adminService.addBlog(formData).subscribe({
      next: () => {
        this.toastr.success("Added successfuly", null, {
          positionClass: 'toast-bottom-center'
        });
        this.ResetForm();
        this.loadBlogs();
      },
      error: err => {
        console.log(err);
      }
    });

  }

  searchFilter() {
    console.log('entered');
    this.blogs = this.blogsSrc.filter(b => {
      return b.title.includes(this.searchText);
    });
  }

  loadBlogs() {
    this.userService.getBlogs().subscribe(result => {
      this.blogsSrc = result;
      this.blogs = this.blogsSrc.slice();
    });
  }

  contentCut(content: string) {
    return content.substring(0, 50) + '...';
  }

  onDeleteBlog(id: number) {
    const confirmed = confirm("Are you sure");
    if (confirmed) {
      this.adminService.deleteBlog(id).subscribe({
        next: () => {
          this.toastr.warning("Blog deleted", null, {
            positionClass: 'toast-bottom-center'
          });
          this.blogsSrc = this.blogsSrc.filter(b => b.id !== id);
          this.blogs = this.blogsSrc.slice();
        },
        error: err => {
          console.log(err);
        }
      });
    }
  }

  ResetForm() {
    this.fileValue = null;
    this.invalidFile = false;
    this.addBlogForm.reset();
  }

  isImage(file: File) :boolean {
    return file.type.includes("image");
  }
}
