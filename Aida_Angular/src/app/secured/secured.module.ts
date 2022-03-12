import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SecuredRoutingModule } from './secured-routing.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AngularMaterialModule} from '../shared/angular-material.module';
import { FontIconModule } from '../shared/fonticon.module';
import { LayoutComponent } from '../shared/layout/layout.component';
import { TopNavComponent } from '../shared/layout/top-nav/top-nav.component';
import { SideNavComponent } from '../shared/layout/side-nav/side-nav.component';
import { SideNavItemComponent } from '../shared/layout/side-nav-item/side-nav-item.component';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

@NgModule({
  declarations: [LayoutComponent, TopNavComponent, SideNavComponent, SideNavItemComponent],
  imports: [
    CommonModule,
    AngularMaterialModule,
    FontIconModule,
    PerfectScrollbarModule,
    NgxChartsModule,
    SecuredRoutingModule
  ],
  providers: [
    PerfectScrollbarModule,
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
    }
  ]

})
export class SecuredModule { }
