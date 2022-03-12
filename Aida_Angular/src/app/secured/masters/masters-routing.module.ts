import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CompanyComponent } from './company/company.component';
import { EmployeeComponent } from './employee/employee.component';
import { DocumentComponent } from './document/document.component';
import { ClientProfileComponent } from './client-profile/client-profile.component';
import { ClientOfficerComponent } from './client-officer/client-officer.component';
import { ServiceDefinitionComponent } from './service-definition/service-definition.component';
import { ServicesSOPComponent } from './services-sop/services-sop.component';
import { EntityShareholdersComponent } from './entity-shareholders/entity-shareholders.component';
import { DownloadFormsComponent } from './download-forms/download-forms.component';
import { ManageMasterComponent } from './manage-master/manage-master.component';




const routes: Routes = [
  { path: 'company', component: CompanyComponent},
  { path: 'employee', component: EmployeeComponent},
  { path: 'document', component: DocumentComponent},
  { path: 'client-profile', component: ClientProfileComponent},
  { path: 'client-officer/:businessProfileId/:businessProfileName', component: ClientOfficerComponent},
  { path: 'entity-shareholders/:businessProfileId/:businessProfileName', component: EntityShareholdersComponent},
  { path: 'service-definition', component: ServiceDefinitionComponent},
  { path: 'service-sop/:serviceCode', component: ServicesSOPComponent},
  { path: 'download-forms/:serviceCode', component: DownloadFormsComponent},
  { path: 'manage-master/:master', component: ManageMasterComponent},



];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MastersRoutingModule { }
