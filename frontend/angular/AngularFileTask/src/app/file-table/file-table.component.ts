import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { FileContent } from '../contracts/file/file-content';
import { FileService } from '../services/file-service/file.service';
import { FileInfo } from '../contracts/file/file-info';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-file-table',
  templateUrl: './file-table.component.html',
  styleUrls: ['./file-table.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class FileTableComponent implements OnInit {

  @Input() file: FileInfo;

  fileContent: FileContent = null;  

  constructor(private fs: FileService, private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.fs.loadFile(this.file)
      .subscribe(result => this.fileContent = result);
  }

  addRow(): void {
    if (this.fileContent && this.indexOfEmpty() < 0) {      
      this.fileContent.content.push([]);
    }
  }

  saveData(): void {
    if (this.fileContent && this.indexOfEmpty() >= 0) {
      this.fileContent.content = this.fileContent.content.splice(this.indexOfEmpty(), 1);
    }

    this.fs.saveFile(this.file, this.fileContent).subscribe(result => {
      const config = {
        duration: 3000
      };

      this.snackBar.open("Статус", result ? "OK!" : "Error!", config);
    });    
  }
  
  private indexOfEmpty() : number {
    if (this.fileContent) {
      return this.fileContent.content.findIndex(row => 
          row.filter(col => col.trim() == "").length == row.length)          
    }
    return -1;
  }

}
