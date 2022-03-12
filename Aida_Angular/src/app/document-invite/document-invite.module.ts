import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { FlexLayoutModule } from '@angular/flex-layout';
import {AngularMaterialModule} from '../shared/angular-material.module';

import { DocumentInviteRoutingModule } from './document-invite-routing.module';
import { DocumentInviteComponent } from './document-invite.component';


@NgModule({
  declarations: [DocumentInviteComponent],
  imports: [
    CommonModule,
    FormsModule,
    FlexLayoutModule,
    AngularMaterialModule, 
    DocumentInviteRoutingModule
  ]
})
export class DocumentInviteModule { }
