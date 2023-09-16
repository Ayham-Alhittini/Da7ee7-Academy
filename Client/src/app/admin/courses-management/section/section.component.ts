import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Section } from 'src/app/models/section';
import { ManageCoursesService } from 'src/app/services/manage-courses.service';

@Component({
  selector: 'app-section',
  templateUrl: './section.component.html',
  styleUrls: ['./section.component.css']
})
export class SectionComponent  implements OnInit{
  
  /**
   * This component contain more than one functionality
   * it have a a detail about the section including section items
   * 
   * it also have add new section item modal
   * 
   */

  @ViewChild('videoElement') videoElement: ElementRef;
  @ViewChild('closeBtn') closeBtn: ElementRef;
  @ViewChild('resetBtn') resetBtn: ElementRef;

  sectionId = null;
  section: Section = null;

  invalidFileUploaded = false;
  submitted = false;
  loading = false;
  deleteLoading = false;
  previewVideoFlag = false;

  newSectionItem = {
    sectionId : null,
    sectionItemTitle: null,
    videoLength: null,
    file: null
  };


  constructor(private route: ActivatedRoute, 
    private manageCoursesService: ManageCoursesService, 
    private toaster: ToastrService, 
    private router: Router) {
    const sectionId = +route.snapshot.paramMap.get('id');
    this.sectionId = sectionId;
    this.newSectionItem.sectionId = sectionId;
    this.newSectionItem.videoLength = 0;
  }

  ngOnInit(): void {
    this.loadSection();
  }
  
  private loadSection() {
    if (this.sectionId) {
      this.manageCoursesService.getCourseSection(this.sectionId).subscribe(result => {
        this.section = result;
      });
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    this.newSectionItem.file = file;
    this.previewVideoFlag = false;

    if (!file) {
      this.invalidFileUploaded = true;
      return;
    } else {
      this.invalidFileUploaded = false;
    }


    const isLecture = this.isLecture(file);
    const isAttachment = this.isAttachment(file);

    if (!isLecture && !isAttachment) {
      this.invalidFileUploaded = true;
      return;
    }

    if (isLecture) {
      this.previewVideo(file);
    } else {
      this.newSectionItem.videoLength = 0;
    }

  }


  onAddSectionItemSubmit() {

    this.submitted = true;
    
    if (this.invalidFileUploaded || this.newSectionItem.sectionItemTitle === null) {
      return;
    }

    if (!this.newSectionItem.file) {
      this.invalidFileUploaded = true;
      return;
    }
    const formData = new FormData();

    const videoLength = ''+ Math.floor(this.newSectionItem.videoLength);
    formData.append('SectionId', this.newSectionItem.sectionId);
    formData.append('SectionItemTitle', this.newSectionItem.sectionItemTitle);
    formData.append('VideoLength', videoLength);
    formData.append('File', this.newSectionItem.file);

    this.loading = true;
    this.manageCoursesService.addCourseSectionItem(formData).subscribe({
      next: () => {
        this.toaster.success("Content added successfuly 👍");
        this.loading = false;
        setTimeout(() => {
          this.resetBtn.nativeElement.click();
          // this.closeBtn.nativeElement.click();
          this.loadSection();
        }, 1000);
      },
      error : err => {
        console.log(err);
        this.loading = false;
      }
    });
  }

  deleteSection() {
    const response = confirm("Are you sure, the section will be deleted");
    if (response && this.section) {
      this.deleteLoading = true;
      this.manageCoursesService.deleteCourseSection(this.section.id).subscribe(() => {
        this.router.navigateByUrl("/courses-management/course/" + this.section.courseId);
      })
      this.deleteLoading = false;
    }
  }

  onReset() {
    this.invalidFileUploaded = false;
    this.submitted = false;
    this.previewVideoFlag = false;

    this.newSectionItem = {
      sectionId: this.sectionId,
      sectionItemTitle: null,
      videoLength: 0,
      file: null
    };

  }

  private isLecture(file: File): boolean {
    return file?.name.endsWith(".mp4");
  }
  private isAttachment(file: File): boolean {
    return file?.name.endsWith(".pdf");
  }
  private previewVideo(file : File) {
    if (file) {
      // Create a URL for the selected video file
      const videoURL = URL.createObjectURL(file);

      // Set the video source to the URL
      this.videoElement.nativeElement.src = videoURL;

      // Load the video metadata to get its duration
      this.videoElement.nativeElement.onloadedmetadata = () => {
        this.previewVideoFlag = true;
        this.newSectionItem.videoLength = this.videoElement.nativeElement.duration;
      };
    }
  }
}
