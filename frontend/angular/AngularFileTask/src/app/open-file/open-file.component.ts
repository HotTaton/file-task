import { Component, OnInit } from '@angular/core';
import { FileService } from '../services/file-service/file.service';
import { FlatTreeControl } from '@angular/cdk/tree';
import { DynamicFlatNode, DynamicDataSource } from './dynamic-data-source';

@Component({
  selector: 'app-open-file',
  templateUrl: './open-file.component.html',
  styleUrls: ['./open-file.component.css']
})
export class OpenFileComponent implements OnInit {
  
  constructor(fs: FileService) { 
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

}
