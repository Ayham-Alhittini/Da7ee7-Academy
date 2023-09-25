import {CdkDragDrop, CdkDropList, CdkDrag, moveItemInArray} from '@angular/cdk/drag-drop';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Section } from 'src/app/models/section';
import { SectionItem } from 'src/app/models/sectionItem';
import { ManageCoursesService } from 'src/app/services/manage-courses.service';

@Component({
  selector: 'app-section',
  templateUrl: './section.component.html',
  styleUrls: ['./section.component.css']
})
export class SectionComponent  implements OnInit{
  
  /**
   * This component contain (4) functionality
   * 
   * 1 - show section items list
   * 2 - delete section
   * 3 - add new section item
   * 4 - edit section item
   * 5 - reorder the section items
   */

  @ViewChild('videoElement') videoElement: ElementRef;
  @ViewChild('editMoadlVideo') editMoadlVideo: ElementRef;
  @ViewChild('resetBtn') resetBtn: ElementRef;
  @ViewChild('editCloseBtn') editCloseBtn: ElementRef;

  sectionId = null;
  section: Section = null;

  OnlyNULL = null;///to erase the file name when close the edit modal

  orderChanged = false;

  addSectionItemModal = {
    invalidFileUploaded : false,
    submitted: false,
    loading: false,
    previewVideoFlag: false,
  }

  deleteLoading = false;
  editLoading = false;
  
  activeSectionItem = {
    id: null,
    sectionItemTitle: null,
    contentUrl: null,
    type: null,
    videoLength: null,
    file: null
  };

  newSectionItem = {
    sectionId : null,
    sectionItemTitle: null,
    videoLength: null,
    file: null
  };


  drop(event: CdkDragDrop<string[]>) {
    this.orderChanged = true;
    moveItemInArray(this.section.sectionItems, event.previousIndex, event.currentIndex);
  }

  constructor(route: ActivatedRoute, 
    private manageCoursesService: ManageCoursesService, 
    private toaster: ToastrService, 
    private router: Router) {
    const sectionId = +route.snapshot.paramMap.get('id');
    this.sectionId = sectionId;
    this.newSectionItem.sectionId = sectionId;
    this.newSectionItem.videoLength = 0;
    this.activeSectionItem.videoLength = 0;
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

  onEditSectionTitle() {
    this.manageCoursesService.editSectionTitle(this.section.id, this.section.sectionTitle).subscribe(() => {
      this.toaster.success('Section title edited successfuly');
    });
  }

  onSectionItemClick(sectionItem: SectionItem) {
    this.OnlyNULL = null;
    this.activeSectionItem = {...sectionItem, file: null};
  }


  onFileSelected(event: any) {
    const file = event.target.files[0];
    this.newSectionItem.file = file;
    this.addSectionItemModal.previewVideoFlag = false;

    if (!file) {
      this.addSectionItemModal.invalidFileUploaded = true;
      return;
    } else {
      this.addSectionItemModal.invalidFileUploaded = false;
    }


    const isLecture = this.isLecture(file);
    const isAttachment = this.isAttachment(file);

    if (!isLecture && !isAttachment) {
      this.addSectionItemModal.invalidFileUploaded = true;
      return;
    }

    if (isLecture) {
      this.previewAddVideo(file);
    } else {
      this.newSectionItem.videoLength = 0;
    }

  }


  onAddSectionItemSubmit() {

    this.addSectionItemModal.submitted = true;
    
    if (this.addSectionItemModal.invalidFileUploaded || this.newSectionItem.sectionItemTitle === null) {
      return;
    }

    if (!this.newSectionItem.file) {
      this.addSectionItemModal.invalidFileUploaded = true;
      return;
    }
    const formData = new FormData();

    const videoLength = ''+ Math.floor(this.newSectionItem.videoLength);
    formData.append('SectionId', this.newSectionItem.sectionId);
    formData.append('SectionItemTitle', this.newSectionItem.sectionItemTitle);
    formData.append('VideoLength', videoLength);
    formData.append('File', this.newSectionItem.file);

    this.addSectionItemModal.loading = true;
    this.manageCoursesService.addCourseSectionItem(formData).subscribe({
      next: () => {
        this.toaster.success("Content added successfuly 👍");
        this.addSectionItemModal.loading = false;
        setTimeout(() => {
          this.resetBtn.nativeElement.click();
          // this.closeBtn.nativeElement.click();
          this.loadSection();
        }, 1000);
      },
      error : err => {
        console.log(err);
        this.addSectionItemModal.loading = false;
      }
    });
  }

  onEditSectionItemSubmit() {
    if (this.invalidSectionItemTitle(this.activeSectionItem)) {
      return;
    }
    
    const formData = new FormData();
    const videoLength = ''+ Math.floor(this.activeSectionItem.videoLength);
    formData.append('Id', this.activeSectionItem.id);
    formData.append('SectionItemTitle', this.activeSectionItem.sectionItemTitle);
    formData.append('VideoLength', videoLength);
    formData.append('File', this.activeSectionItem.file);

    this.editLoading = true;
    this.manageCoursesService.editSectionItem(formData).subscribe({
      next: () => {
        this.toaster.success("Updated successfuly");
        this.editLoading = false;

        //an alternative to refetch the section
        ///get the section item from section and edit it
        var sectionItem = this.section.sectionItems.find(si => si.id === this.activeSectionItem.id);

        sectionItem.id = this.activeSectionItem.id;
        sectionItem.sectionItemTitle = this.activeSectionItem.sectionItemTitle;
        sectionItem.contentUrl = this.activeSectionItem.contentUrl;
        sectionItem.type = this.activeSectionItem.type;
        

        setTimeout(() => {
          this.editCloseBtn.nativeElement.click();
        }, 500);
      },
      error: err => {
        this.toaster.error('Error occur while updating');
        console.log(err);
        this.editLoading = false;
      }
    })
    
  }

  onEditSectionItemFileSelected(event: any) {

    const file = event.target.files[0];
    
    if (!file) {
      return;
    }
    
    const isLecture = this.isLecture(file);
    const isAttachment = this.isAttachment(file);

    if (!isLecture && !isAttachment) {
      this.toaster.error("Unsupported media expect only mp4 for lectures and pdf for attachments", "File not taken", {
        timeOut: 5000
      });
      return;
    }

    this.activeSectionItem.file = file;
    if (isLecture) {
      this.previewEditVideo(file);
    } else {
      this.activeSectionItem.type = 1;
      this.activeSectionItem.videoLength = 0;
      this.activeSectionItem.contentUrl = URL.createObjectURL(file);
    }
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
    this.addSectionItemModal.invalidFileUploaded = false;
    this.addSectionItemModal.submitted = false;
    this.addSectionItemModal.previewVideoFlag = false;

    this.newSectionItem = {
      sectionId: this.sectionId,
      sectionItemTitle: null,
      videoLength: 0,
      file: null
    };

  }


  saveOrderChanges() {

    const newOrder = {
      sectionId: this.section.id,
      sectionItemsIds: []
    };

    for (let index = 0; index < this.section.sectionItems.length; index++) {
      const element = this.section.sectionItems[index];
      newOrder.sectionItemsIds.push(element.id);
    }

    this.manageCoursesService.editSectionItemsOrder(newOrder).subscribe(() => {
      this.toaster.show("Changes Saved", null, {
        positionClass: 'toast-bottom-center'
      })
      this.orderChanged = false;
    });
  }


  ///validation for edit modal
  invalidSectionItemTitle(source: any) {
    return source.sectionItemTitle === null ||
           source.sectionItemTitle === '';
  }


  private isLecture(file: File): boolean {
    return file?.name.endsWith(".mp4");
  }
  private isAttachment(file: File): boolean {
    return file?.name.endsWith(".pdf");
  }
  private previewAddVideo(file : File) {
    if (file) {
      // Create a URL for the selected video file
      const videoURL = URL.createObjectURL(file);

      // Set the video source to the URL
      this.videoElement.nativeElement.src = videoURL;

      // Load the video metadata to get its duration
      this.videoElement.nativeElement.onloadedmetadata = () => {
        this.addSectionItemModal.previewVideoFlag = true;
        this.newSectionItem.videoLength = this.videoElement.nativeElement.duration;
      };
    }
  }
  private previewEditVideo(file : File) {
    if (file) {
      // Create a URL for the selected video file
      const videoURL = URL.createObjectURL(file);

      // Set the video source to the URL
      this.activeSectionItem.contentUrl = videoURL;

      // Load the video metadata to get its duration
      this.editMoadlVideo.nativeElement.onloadedmetadata = () => {
        this.activeSectionItem.videoLength = this.editMoadlVideo.nativeElement.duration;
        this.activeSectionItem.type = 2;
      };
    }
  }
}
