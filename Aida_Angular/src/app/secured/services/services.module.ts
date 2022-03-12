import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularMaterialModule} from '../../shared/angular-material.module'
import { FontIconModule } from '../../shared/fonticon.module';
import { SharedModule } from '../../shared/shared.module'
import { FlexLayoutModule } from '@angular/flex-layout';
import { PdfViewerModule } from 'ng2-pdf-viewer';

import { ServicesRoutingModule } from './services-routing.module';
import { OnboardingComponent } from './onboarding/onboarding.component';
import { RegistrationComponent } from './registration/registration.component';
import { ListingComponent } from './listing/listing.component';
import { ExecutionComponent } from './execution/execution.component';
import { SigningComponent } from './signing/signing.component';
import { UploadComponent } from './upload/upload.component';
import { ClientsComponent } from './clients/clients.component';
import { RegistrationOptionsComponent } from './registration-options/registration-options.component';

import {AngularTreeGridModule} from 'angular-tree-grid';
import { EmploymentAgencyComponent } from './employment-agency/employment-agency.component';

import { ChecklistComponent } from './checklist/checklist.component';
import { ChecklistFormComponent } from './checklist-form/checklist-form.component';

@NgModule({
  declarations: [OnboardingComponent, RegistrationComponent, ListingComponent, ExecutionComponent, SigningComponent,
     UploadComponent, ClientsComponent, RegistrationOptionsComponent, EmploymentAgencyComponent, ChecklistComponent, ChecklistFormComponent],
  imports: [
    CommonModule,
    FormsModule,
    AngularMaterialModule, 
    FontIconModule,
    FlexLayoutModule,
    SharedModule,
    PdfViewerModule,
    AngularTreeGridModule,
    ServicesRoutingModule
  ]
})
export class ServicesModule { }
