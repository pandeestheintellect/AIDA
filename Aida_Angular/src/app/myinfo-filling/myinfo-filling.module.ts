import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MyinfoFillingRoutingModule } from './myinfo-filling-routing.module';
import { InviteComponent } from './invite/invite.component';
import { FillingComponent } from './filling/filling.component';

import { SharedModule } from '../shared/shared.module'
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
    MyinfoFillingRoutingModule 
  ]
})
export class MyinfoFillingModule { }
