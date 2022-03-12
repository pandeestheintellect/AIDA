import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularMaterialModule} from '../../shared/angular-material.module'
import { FontIconModule } from '../../shared/fonticon.module';
import { SharedModule } from '../../shared/shared.module'
import { FlexLayoutModule } from '@angular/flex-layout';

import { ClientDashboardRoutingModule } from './client-dashboard-routing.module';
import { ClientDashboardComponent } from './client-dashboard.component';


@NgModule({
  declarations: [ClientDashboardComponent],
  imports: [
    CommonModule,
    FormsModule,
    AngularMaterialModule,
    FontIconModule,
    FlexLayoutModule, 
    ReactiveFormsModule,
    SharedModule,
    ClientDashboardRoutingModule
  ]
})
export class ClientDashboardModule { }
