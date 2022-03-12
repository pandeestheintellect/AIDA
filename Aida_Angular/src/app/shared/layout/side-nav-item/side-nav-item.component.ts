import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-side-nav-item',
  templateUrl: './side-nav-item.component.html',
  styleUrls: ['./side-nav-item.component.scss']
})
export class SideNavItemComponent implements OnInit {

  @Input() menu;
  @Input() iconOnly: boolean;
  @Input() secondaryMenu = false;

  constructor() { }

  ngOnInit(): void {
  }

  openLink() {
    this.menu.open = this.menu.open;
  }

  chechForChildMenu() {
      return (this.menu && this.menu?.sub) ? true : false;
  }
}
