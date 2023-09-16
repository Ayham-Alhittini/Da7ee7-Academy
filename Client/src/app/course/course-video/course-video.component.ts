import { Component, Input } from '@angular/core';
import { VgApiService } from '@videogular/ngx-videogular/core';

@Component({
  selector: 'app-course-video',
  templateUrl: './course-video.component.html',
  styleUrls: ['./course-video.component.css']
})
export class CourseVideoComponent {

  @Input('src') src: string = null;
  api: VgApiService = new VgApiService;

  onPlayerReady(source: VgApiService) {
    this.api = source;
    this.api.getDefaultMedia().subscriptions.loadedMetadata.subscribe(
      // this.autoplay.bind(this)
    )
  }
  autoplay() {
    this.api.play();
  }

}
