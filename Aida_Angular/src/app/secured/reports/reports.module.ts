import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularMaterialModule} from '../../shared/angular-material.module'
import { FontIconModule } from '../../shared/fonticon.module';
import { SharedModule } from '../../shared/shared.module'
import { FlexLayoutModule } from '@angular/flex-layout';
import { PdfViewerModule } from 'ng2-pdf-viewer';


import { ReportsRoutingModule } from './reports-routing.module';
import { EnquirySummaryComponent } from './enquiry-summary/enquiry-summary.component';
import { UploadedDocumentsComponent } from './uploaded-documents/uploaded-documents.component';
import { SignedDocumentsComponent } from './signed-documents/signed-documents.component';
import { ServicesSummaryComponent } from './services-summary/services-summary.component';


@NgModule({
  declarations: [EnquirySummaryComponent, UploadedDocumentsComponent, SignedDocumentsComponent, ServicesSummaryComponent],
  imports: [
    CommonModule,
    FormsModule,
    AngularMaterialModule, 
    FontIconModule,
    FlexLayoutModule,
    SharedModule,
    ReportsRoutingModule
  ]
})
export class ReportsModule { }
