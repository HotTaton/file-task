import { Component, OnInit } from '@angular/core';
import { FileService } from '../services/file-service/file.service';
import { FlatTreeControl } from '@angular/cdk/tree';
import { DynamicFlatNode, DynamicDataSource } from './dynamic-data-source';
import { FileProcessorComponent } from '../file-processor/file-processor.component';
import { MatDialogRef } from '@angular/material/dialog';
import { FileInfo } from '../contracts/file/file-info';

@Component({
  selector: 'app-open-file',
  templateUrl: './open-file.component.html',
  styleUrls: ['./open-file.component.css']
})
export class OpenFileComponent implements OnInit {

  private selectedNode: FileInfo = null;
  
  constructor(fs: FileService, public dialogRef: MatDialogRef<FileProcessorComponent>) { 
    this.treeControl = new FlatTreeControl<DynamicFlatNode>(this.getLevel, this.isExpandable);
    this.dataSource = new DynamicDataSource(this.treeControl, fs);

    this.dataSource.initialData();
  }

  ngOnInit(): void {    
  }

  treeControl: FlatTreeControl<DynamicFlatNode>;

  dataSource: DynamicDataSource;

  getLevel = (node: DynamicFlatNode) => node.level;

  isExpandable = (node: DynamicFlatNode) => node.expandable;

  hasChild = (_: number, _nodeData: DynamicFlatNode) => _nodeData.expandable;

  onNoClick(): void {
    this.dialogRef.close();
  }

  onOkClick(): void {
    this.dialogRef.close({ selectedFile: this.selectedNode });
  }

  selectNode(node: FileInfo): void {
    this.selectedNode = node;
  }

  isSelected(node: FileInfo): string {
    if (node == this.selectedNode) {
      return "accent";
    }
    return "";
  }

}
