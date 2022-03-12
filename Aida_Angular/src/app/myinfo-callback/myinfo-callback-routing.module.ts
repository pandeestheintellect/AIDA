import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MyinfoCallbackComponent } from './myinfo-callback.component';

const routes: Routes = [{
  path: '',
  component: MyinfoCallbackComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyinfoCallbackRoutingModule { }
