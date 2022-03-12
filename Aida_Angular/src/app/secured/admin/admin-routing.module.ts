import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../../service/auth.guard';

import { ArchivalComponent } from './archival/archival.component';

const routes: Routes = [
  { path: 'archival', component: ArchivalComponent, canActivate: [AuthGuard]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
