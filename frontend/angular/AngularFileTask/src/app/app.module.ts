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
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; 
import { MatButtonModule } from '@angular/material/button';
import { FileProcessorComponent } from './file-processor/file-processor.component';
import { FileTableComponent } from './file-table/file-table.component';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { TableModule } from 'primeng/table';
import {MatSnackBarModule} from '@angular/material/snack-bar';

@NgModule({
  imports: [
    HttpClientModule,
    BrowserModule,
    ReactiveFormsModule,
    FormsModule,
    BrowserAnimationsModule,
    MatTreeModule,
    MatIconModule,
    MatProgressBarModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatDialogModule,
    MatButtonToggleModule,
    TableModule,
    MatSnackBarModule,
    RouterModule.forRoot([
      { path: '', component: FileProcessorComponent, pathMatch: 'full' }
    ])
  ],

  declarations: [
    AppComponent,
    OpenFileComponent,
    FileProcessorComponent,
    FileTableComponent
  ],

  entryComponents: [
    FileProcessorComponent,
    OpenFileComponent
  ],
  
  providers: [
    {provide: "BASE_API_URL", useValue: environment.apiUrl},
    httpInterceptorProviders
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
