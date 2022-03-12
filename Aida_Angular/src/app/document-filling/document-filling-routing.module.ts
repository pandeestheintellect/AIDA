

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { InviteComponent } from './invite/invite.component';
import { FillingComponent } from './filling/filling.component';

const routes: Routes = [
  {path: 'invite/:key', component: InviteComponent},
  {path: 'filling', component: FillingComponent}
  ]
  ;

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentFillingRoutingModule { }
