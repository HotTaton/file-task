import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { OpenFileComponent } from '../open-file/open-file.component';
import { FileInfo } from '../contracts/file/file-info';

@Component({
  selector: 'app-file-processor',
  templateUrl: './file-processor.component.html',
  styleUrls: ['./file-processor.component.css']
})
export class FileProcessorComponent implements OnInit {

  public selectedFile: FileInfo = null;

  mainForm = new FormGroup({
    fileNameControl: new FormControl("", null)
  });

  constructor(private dialog: MatDialog) { }

  ngOnInit() {
  }

  public get fileName(): string {
    if (this.selectedFile != null) {
      return this.selectedFile.formattedName;
    }
    return "";
  }

  openDialog(): void {

    const dialogRef = this.dialog.open(OpenFileComponent, {
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null && result !== undefined) {
        this.selectedFile = result.selectedFile;
      }
    });
  }
}
