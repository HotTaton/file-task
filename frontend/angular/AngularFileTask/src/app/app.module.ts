import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { OpenFileComponent } from './open-file/open-file.component';
import { environment } from 'src/environments/environment';
import { httpInterceptorProviders } from './interceptors/interceptor-providers';
import { MatTreeModule } from '@angular/material/tree';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; 
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  imports: [
    HttpClientModule,
    BrowserModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatTreeModule,
    MatIconModule,
    MatProgressBarModule,
    MatButtonModule,
    RouterModule.forRoot([
      { path: '', component: OpenFileComponent, pathMatch: 'full' }
    ])
  ],

  declarations: [
    AppComponent,
    OpenFileComponent
  ],
  
  providers: [
    {provide: "BASE_API_URL", useValue: environment.apiUrl},
    httpInterceptorProviders
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
