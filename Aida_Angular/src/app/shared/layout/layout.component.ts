import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { MediaObserver, MediaChange } from '@angular/flex-layout';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html'
})
export class LayoutComponent implements OnInit, OnDestroy {
  @Input() isVisible : boolean = true;
  visibility = 'shown';

  sideNavOpened: boolean = true;
  matDrawerOpened: boolean = false;
  matDrawerShow: boolean = true;
  sideNavMode: string = 'side';
  
  ngOnChanges() {
   this.visibility = this.isVisible ? 'shown' : 'hidden';
  }
  private readonly mediaWatcher: Subscription;

  constructor(media: MediaObserver) 
  {
    this.mediaWatcher = media.media$.subscribe((change: MediaChange) => {
      
     if (change.mqAlias === 'xl' || change.mqAlias === 'lg' || change.mqAlias === 'md') {
      this.sideNavMode = 'side';
      this.sideNavOpened = true;
      this.matDrawerOpened = false;
      this.matDrawerShow = true;
      
    } else if(change.mqAlias === 'sm') {
        this.sideNavMode = 'side';
        this.sideNavOpened = false;
        this.matDrawerOpened = true;
        this.matDrawerShow = true;
    } else if (change.mqAlias === 'xs') {
        this.sideNavMode = 'over';
        this.sideNavOpened = false;
        this.matDrawerOpened = false;
        this.matDrawerShow = false;
    }
    
    });
  }
  ngOnInit() { }
  getRouteAnimation(outlet) {

      return outlet.activatedRouteData.animation;
  }
  ngOnDestroy(): void {
    this.mediaWatcher.unsubscribe();
  }
}
