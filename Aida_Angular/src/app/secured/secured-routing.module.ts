import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../service/auth.guard';

import { LayoutComponent } from '../shared/layout/layout.component';

const routes: Routes = [{
  path: '',
  component: LayoutComponent,
  children: [
    {path: 'dashboards',loadChildren: () => import('./dashboards/dashboards.module').then(m => m.DashboardsModule), canActivate: [AuthGuard]},
    {path: 'masters',loadChildren: () => import('./masters/masters.module').then(m => m.MastersModule) , canActivate: [AuthGuard]},
    {path: 'registers',loadChildren: () => import('./registers/registers.module').then(m => m.RegistersModule) , canActivate: [AuthGuard]},
    {path: 'services',loadChildren: () => import('./services/services.module').then(m => m.ServicesModule) , canActivate: [AuthGuard]},
    {path: 'reports',loadChildren: () => import('./reports/reports.module').then(m => m.ReportsModule) , canActivate: [AuthGuard]},
    {path: 'admin',loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule) , canActivate: [AuthGuard]},
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SecuredRoutingModule { }
