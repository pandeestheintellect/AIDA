import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularMaterialModule} from '../shared/angular-material.module'
import { FontIconModule } from '../shared/fonticon.module';
import { SharedModule } from '../shared/shared.module'
import { FlexLayoutModule } from '@angular/flex-layout';

import { MyinfoCallbackRoutingModule } from './myinfo-callback-routing.module';
import { MyinfoCallbackComponent } from './myinfo-callback.component';


@NgModule({
  declarations: [MyinfoCallbackComponent],
  imports: [
    CommonModule,
    FormsModule,
    AngularMaterialModule,
    FontIconModule,
    FlexLayoutModule,
    ReactiveFormsModule,
    SharedModule,
    MyinfoCallbackRoutingModule
  ]
})
export class MyinfoCallbackModule { }
