import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';



import { EnquirySummaryComponent } from './enquiry-summary/enquiry-summary.component';
import { UploadedDocumentsComponent } from './uploaded-documents/uploaded-documents.component';
import { SignedDocumentsComponent } from './signed-documents/signed-documents.component';
import { ServicesSummaryComponent } from './services-summary/services-summary.component';

const routes: Routes = [
  { path: 'enquiry-summary/:periods/:status/:displayName', component: EnquirySummaryComponent},
  { path: 'uploaded-documents/:businessProfileId/:businessProfileName', component: UploadedDocumentsComponent},
  { path: 'signed-documents/:businessProfileId/:businessProfileName', component: SignedDocumentsComponent},
  { path: 'services-summary', component: ServicesSummaryComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }
