import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularMaterialModule} from '../../shared/angular-material.module'
import { FontIconModule } from '../../shared/fonticon.module';
import { SharedModule } from '../../shared/shared.module'
import { FlexLayoutModule } from '@angular/flex-layout';


import { DashboardsRoutingModule } from './dashboards-routing.module';
import { CompanyViewComponent } from './company-view/company-view.component';
import { ClientViewComponent } from './client-view/client-view.component';
import { CardClientSummaryComponent } from './card-client-summary/card-client-summary.component';
import { CardClientProfileComponent } from './card-client-profile/card-client-profile.component';
import { CardClientServicesComponent } from './card-client-services/card-client-services.component';
import { CardServicesStatusComponent } from './card-services-status/card-services-status.component';
import { CardServicesPerformanceComponent } from './card-services-performance/card-services-performance.component';
import { CardServicesSummaryComponent } from './card-services-summary/card-services-summary.component';
import { CardActiveProfileComponent } from './card-active-profile/card-active-profile.component';
import { CardEventsInfoComponent } from './card-events-info/card-events-info.component';


@NgModule({
  declarations: [CompanyViewComponent, ClientViewComponent,CardClientSummaryComponent, CardClientProfileComponent, CardClientServicesComponent, CardServicesStatusComponent, CardServicesPerformanceComponent, CardServicesSummaryComponent, CardActiveProfileComponent, CardEventsInfoComponent],
  imports: [
    CommonModule,
    FormsModule,
    AngularMaterialModule,
    FontIconModule,
    FlexLayoutModule, 
    ReactiveFormsModule,
    SharedModule,
    DashboardsRoutingModule
  ]
})
export class DashboardsModule { }
