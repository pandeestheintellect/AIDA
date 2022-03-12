import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularMaterialModule} from '../../shared/angular-material.module'
import { FontIconModule } from '../../shared/fonticon.module';
import { SharedModule } from '../../shared/shared.module'
import { FlexLayoutModule } from '@angular/flex-layout';
import {AngularTreeGridModule} from 'angular-tree-grid';
import { RegistersRoutingModule } from './registers-routing.module';
import { ApplicationsAllotmentsComponent } from './applications-allotments/applications-allotments.component';
import { OfficersComponent } from './officers/officers.component';
import { SecretariesComponent } from './secretaries/secretaries.component';
import { DirectorsShareholdingsComponent } from './directors-shareholdings/directors-shareholdings.component';
import { TransfersComponent } from './transfers/transfers.component';
import { ManagingDirectorsComponent } from './managing-directors/managing-directors.component';
import { AuditorsComponent } from './auditors/auditors.component';
import { MortgagesChargesComponent } from './mortgages-charges/mortgages-charges.component';
import { ApplicationsAllotmentsDetailComponent } from './applications-allotments-detail/applications-allotments-detail.component';
import { OfficersDetailComponent } from './officers-detail/officers-detail.component';
import { SecretariesDetailComponent } from './secretaries-detail/secretaries-detail.component';
import { DirectorsShareholdingsDetailComponent } from './directors-shareholdings-detail/directors-shareholdings-detail.component';
import { TransfersDetailComponent } from './transfers-detail/transfers-detail.component';
import { ManagingDirectorsDetailComponent } from './managing-directors-detail/managing-directors-detail.component';
import { AuditorsDetailComponent } from './auditors-detail/auditors-detail.component';
import { MortgagesChargesDetailComponent } from './mortgages-charges-detail/mortgages-charges-detail.component';
import { MembersComponent } from './members/members.component';
import { MembersDetailComponent } from './members-detail/members-detail.component';
import { ControllersIndividualsComponent } from './controllers-individuals/controllers-individuals.component';
import { ControllersCorporateComponent } from './controllers-corporate/controllers-corporate.component';
import { ControllersCorporateDetailComponent } from './controllers-corporate-detail/controllers-corporate-detail.component';
import { NomineeNominatorsComponent } from './nominee-nominators/nominee-nominators.component';
import { NomineeNominatorsDetailComponent } from './nominee-nominators-detail/nominee-nominators-detail.component';


@NgModule({
  declarations: [ApplicationsAllotmentsComponent, OfficersComponent, SecretariesComponent, DirectorsShareholdingsComponent, 
    TransfersComponent, ManagingDirectorsComponent, AuditorsComponent, MortgagesChargesComponent, 
    ApplicationsAllotmentsDetailComponent, OfficersDetailComponent, SecretariesDetailComponent, DirectorsShareholdingsDetailComponent, TransfersDetailComponent, ManagingDirectorsDetailComponent, AuditorsDetailComponent, MortgagesChargesDetailComponent, MembersComponent, MembersDetailComponent, ControllersIndividualsComponent, ControllersCorporateComponent, ControllersCorporateDetailComponent, NomineeNominatorsComponent, NomineeNominatorsDetailComponent],
  imports: [
    CommonModule,
    FormsModule,
    AngularMaterialModule,
    FontIconModule,
    FlexLayoutModule,
    ReactiveFormsModule,
    SharedModule,
    AngularTreeGridModule,
    RegistersRoutingModule
   
    
  ]
})
export class RegistersModule { }
