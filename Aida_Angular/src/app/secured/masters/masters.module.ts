import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularMaterialModule} from '../../shared/angular-material.module';
import {AngularTreeGridModule} from 'angular-tree-grid';

import { FontIconModule } from '../../shared/fonticon.module';
import { SharedModule } from '../../shared/shared.module'
import { FlexLayoutModule } from '@angular/flex-layout';
import { MastersRoutingModule } from './masters-routing.module';
import { CompanyComponent } from './company/company.component';
import { CompanyDetailComponent } from './company-detail/company-detail.component';
import { EmployeeComponent } from './employee/employee.component';
import { EmployeeDetailComponent } from './employee-detail/employee-detail.component';
import { DocumentComponent } from './document/document.component';
import { DocumentDetailComponent } from './document-detail/document-detail.component';
import { ClientProfileComponent } from './client-profile/client-profile.component';
import { ClientProfileDetailComponent } from './client-profile-detail/client-profile-detail.component';
import { ClientOfficerComponent } from './client-officer/client-officer.component';
import { ClientOfficerDetailComponent } from './client-officer-detail/client-officer-detail.component';
import { ServiceDefinitionComponent } from './service-definition/service-definition.component';
import { ServiceDefinitionDetailComponent } from './service-definition-detail/service-definition-detail.component';
import { ServicesSOPComponent } from './services-sop/services-sop.component';
import { ServicesSOPDetailComponent } from './services-sop-detail/services-sop-detail.component';
import { ClientActivityComponent } from './client-activity/client-activity.component';
import { ClientMyinfoIntroComponent } from './client-myinfo-intro/client-myinfo-intro.component';
import { ServiceDocumentsComponent } from './service-documents/service-documents.component';
import { ImportDocumentsComponent } from './import-documents/import-documents.component';
import { EntityShareholdersComponent } from './entity-shareholders/entity-shareholders.component';
import { EntityShareholderDetailsComponent } from './entity-shareholder-details/entity-shareholder-details.component';

import { DownloadFormsComponent } from './download-forms/download-forms.component';

import { ManageMasterComponent } from './manage-master/manage-master.component';
import { ManageMasterDetailComponent } from './manage-master-detail/manage-master-detail.component';


@NgModule({
  declarations: [CompanyComponent, CompanyDetailComponent, EmployeeComponent, EmployeeDetailComponent, DocumentComponent,
     DocumentDetailComponent, ClientProfileComponent, ClientProfileDetailComponent, ClientOfficerComponent, 
     ClientOfficerDetailComponent, ServiceDefinitionComponent, ServiceDefinitionDetailComponent, ServicesSOPComponent, 
     ServicesSOPDetailComponent, ClientActivityComponent, ClientMyinfoIntroComponent, ServiceDocumentsComponent, 
     ImportDocumentsComponent, EntityShareholdersComponent, EntityShareholderDetailsComponent, 
      DownloadFormsComponent,

      ManageMasterComponent,

      ManageMasterDetailComponent],
  imports: [
    CommonModule,
    FormsModule,
    AngularMaterialModule,
    FontIconModule,
    FlexLayoutModule,
    ReactiveFormsModule,
    SharedModule,
    AngularTreeGridModule,
    MastersRoutingModule
  ],
  entryComponents: [
    CompanyDetailComponent,EmployeeDetailComponent,DocumentDetailComponent,ClientProfileDetailComponent,
    ClientOfficerDetailComponent,ServiceDefinitionDetailComponent,ServicesSOPDetailComponent,
    ClientActivityComponent,ClientMyinfoIntroComponent
  ]
})
export class MastersModule { }
