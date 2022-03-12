import { Component, OnInit, Input } from '@angular/core';
import { menus } from './menu-element';

import { AuthService } from  '../../../service/auth.service';


@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html'
})
export class SideNavComponent implements OnInit {

  @Input() iconOnly:boolean = false;



  public menus:any[] = [];

  constructor(private auth: AuthService) { }

  ngOnInit(): void {
      this.menus = this.auth.menus;
      
  }

}
