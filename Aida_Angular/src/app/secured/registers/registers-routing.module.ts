import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


import { ApplicationsAllotmentsComponent } from './applications-allotments/applications-allotments.component';
import { OfficersComponent } from './officers/officers.component';
import { SecretariesComponent } from './secretaries/secretaries.component';
import { DirectorsShareholdingsComponent } from './directors-shareholdings/directors-shareholdings.component';
import { TransfersComponent } from './transfers/transfers.component';
import { ManagingDirectorsComponent } from './managing-directors/managing-directors.component';
import { AuditorsComponent } from './auditors/auditors.component';
import { MortgagesChargesComponent } from './mortgages-charges/mortgages-charges.component';
import { MembersComponent } from './members/members.component';
import { NomineeNominatorsComponent } from './nominee-nominators/nominee-nominators.component';
import { ControllersCorporateComponent } from './controllers-corporate/controllers-corporate.component';

const routes: Routes = [
  { path: 'applications-allotments/:clientProfileId', component: ApplicationsAllotmentsComponent},
  { path: 'members/:clientProfileId', component: MembersComponent},
  { path: 'transfers/:clientProfileId', component: TransfersComponent},
  { path: 'officers/:userrole/:clientProfileId', component: OfficersComponent},
  { path: 'auditors/:clientProfileId', component: AuditorsComponent},
  { path: 'directors-shareholdings/:clientProfileId', component: DirectorsShareholdingsComponent},
  { path: 'mortgages-charges/:clientProfileId', component: MortgagesChargesComponent},
  { path: 'controllers-corporate/:clientProfileId', component: ControllersCorporateComponent},
  { path: 'nominee-nominators/:clientProfileId', component: NomineeNominatorsComponent},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegistersRoutingModule { }
