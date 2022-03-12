import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PageNotFoundComponent } from './shared/page-not-found/page-not-found.component';
import { AuthGuard } from './service/auth.guard';

const routes: Routes = [
  {path: '',redirectTo: 'login', pathMatch: 'full'},
  {path: 'login',loadChildren: () => import('./login/login.module').then(m => m.LoginModule)},
  
  {path: 'document-filling',loadChildren: () => import('./document-filling/document-filling.module').then(m => m. DocumentFillingModule)},
  {path: 'myinfo-filling',loadChildren: () => import('./myinfo-filling/myinfo-filling.module').then(m => m. MyinfoFillingModule)},
  {path: 'myinfo-callback',loadChildren: () => import('./myinfo-callback/myinfo-callback.module').then(m => m. MyinfoCallbackModule)},
  {path: 'secured',loadChildren: () => import('./secured/secured.module').then(m => m.SecuredModule),canActivate: [AuthGuard] },
  {path: '**', component: PageNotFoundComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
