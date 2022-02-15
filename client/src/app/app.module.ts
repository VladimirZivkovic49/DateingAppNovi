import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MembersDetailsComponent } from './members/members-details/members-details.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { ToastrModule } from 'ngx-toastr';
import { SharedModule } from './_modules/shared.module';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MembersCardComponent } from './members/members-card/members-card.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import {  NgxSpinnerModule } from 'ngx-spinner';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MembersDetailsComponent,
    ListsComponent,
    MessagesComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MembersCardComponent,
    MemberEditComponent,
    
  ],
  
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
   /*  BsDropdownModule.forRoot(),
    ToastrModule.forRoot({positionClass:'toast-bottom-right'}) */
    SharedModule,
    NgxSpinnerModule
  ],
  providers: [{provide : HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
              {provide : HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
              {provide : HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
              ],
  
  bootstrap: [AppComponent]
})
export class AppModule { }
