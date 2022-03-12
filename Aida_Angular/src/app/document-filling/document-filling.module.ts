import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module'
import { DocumentFillingRoutingModule } from './document-filling-routing.module';
import { InviteComponent } from './invite/invite.component';
import { FillingComponent } from './filling/filling.component';
import { FormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AngularMaterialModule } from '../shared/angular-material.module';


@NgModule({
  declarations: [InviteComponent, FillingComponent],
  imports: [
    CommonModule,
    FormsModule,
    FlexLayoutModule,
    SharedModule,
    AngularMaterialModule,
    DocumentFillingRoutingModule
  ]
})
export class DocumentFillingModule { }
