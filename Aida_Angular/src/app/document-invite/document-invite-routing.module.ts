import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DocumentInviteComponent } from './document-invite.component';

const routes: Routes = [{
  path: '',
  component: DocumentInviteComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentInviteRoutingModule { }
