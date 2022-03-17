import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SigninCallbackComponent } from './callbacks/signin-callback.component';
import { SilentCallbackComponent } from './callbacks/silent-callback.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'signin-callback', component: SigninCallbackComponent },
  { path: 'silent-callback', component: SilentCallbackComponent },
  { path: '404', component: NotFoundComponent },
  { path: '**', redirectTo: '404', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
