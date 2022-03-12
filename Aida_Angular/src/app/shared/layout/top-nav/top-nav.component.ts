import { Component, OnInit, Input } from '@angular/core';

import { AuthService } from  '../../../service/auth.service';

import { Directive, HostListener } from '@angular/core';
import { Location } from '@angular/common';
import { UserIdleService } from 'angular-user-idle';

@Component({
  selector: 'app-top-nav',
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss']
})
export class TopNavComponent implements OnInit {
  
  @Input() sidenav;
	@Input() sidebar;
	@Input() drawer;
  @Input() matDrawerShow;
  
  username:string='';
  timeOut='';
  constructor(private auth: AuthService,private location: Location,private userIdle: UserIdleService) {}

  ngOnInit(): void {
    this.username=this.auth.usernName;

    //Start watching for user inactivity.
    this.userIdle.startWatching();
    
    // Start watching when user idle is starting.
    this.userIdle.onTimerStart().subscribe(count => this.timeOut='You will be logged out in  '+ (60 - count) + ' seconds');
    
    // Start watch when time is up.
    this.userIdle.onTimeout().subscribe(() => this.auth.logout());

  }

  onLoggedout() {
    this.auth.logout();
  }
  onBack()
  {
    this.location.back();
  }
}
