import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


import { CompanyViewComponent } from './company-view/company-view.component';
import { ClientViewComponent } from './client-view/client-view.component';

const routes: Routes = [
  { path: 'company', component: CompanyViewComponent},
  { path: 'client-view', component: ClientViewComponent},
  { path: 'client-view/:businessProfileId', component: ClientViewComponent},
  {path: '**', component: CompanyViewComponent}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardsRoutingModule { }
