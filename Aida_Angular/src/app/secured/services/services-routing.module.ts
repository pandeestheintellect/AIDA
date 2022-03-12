import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OnboardingComponent } from './onboarding/onboarding.component';
import { RegistrationComponent } from './registration/registration.component';
import { RegistrationOptionsComponent } from './registration-options/registration-options.component';
import { ListingComponent } from './listing/listing.component';
import { ExecutionComponent } from './execution/execution.component';
import { ClientsComponent } from './clients/clients.component';
import { EmploymentAgencyComponent } from './employment-agency/employment-agency.component';

import { AuthGuard } from '../../service/auth.guard';
import { ChecklistComponent } from './checklist/checklist.component';
import { ChecklistFormComponent } from './checklist-form/checklist-form.component';

const routes: Routes = [
  { path: 'onboarding', component: OnboardingComponent, canActivate: [AuthGuard]},
  { path: 'registration/:serviceCode', component: RegistrationComponent, canActivate: [AuthGuard]},
  { path: 'registration/:serviceCode/:businessProfileId', component: RegistrationComponent, canActivate: [AuthGuard]},
  { path: 'registration-options/:serviceCode', component: RegistrationOptionsComponent, canActivate: [AuthGuard]},
  { path: 'clients/:serviceCode', component: ClientsComponent, canActivate: [AuthGuard]},
  { path: 'clients/:serviceCode/:status/:period', component: ClientsComponent, canActivate: [AuthGuard]},
  { path: 'clients/:serviceCode/:status/:period/:entity', component: ClientsComponent, canActivate: [AuthGuard]},
  { path: 'admin-clients/:serviceCode', component: ClientsComponent, canActivate: [AuthGuard]},
  { path: 'admin-clients/:serviceCode/:status/:period', component: ClientsComponent, canActivate: [AuthGuard]},
  { path: 'listing', component: ListingComponent, canActivate: [AuthGuard]},
  { path: 'listing/:serviceCode', component: ListingComponent, canActivate: [AuthGuard]},
  { path: 'listing/:serviceCode/:businessProfileId', component: ListingComponent, canActivate: [AuthGuard]},
  { path: 'listing/:serviceCode/:businessProfileId/:status', component: ListingComponent, canActivate: [AuthGuard]},
  { path: 'listing/:serviceCode/:businessProfileId/:status/:serviceBusinessId', component: ListingComponent, canActivate: [AuthGuard]},
  { path: 'execution/:serviceBusinessId/:officerId', component: ExecutionComponent, canActivate: [AuthGuard]},
  { path: 'employment-agency', component: EmploymentAgencyComponent, canActivate: [AuthGuard]},
  { path: 'checklist/:serviceCode', component: ChecklistComponent, canActivate: [AuthGuard]},
  { path: 'checklist-form/:serviceCode', component: ChecklistFormComponent, canActivate: [AuthGuard]},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ServicesRoutingModule { }
