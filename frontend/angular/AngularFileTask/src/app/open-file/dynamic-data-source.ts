import { FileService } from '../services/file-service/file.service';
import { FlatTreeControl } from '@angular/cdk/tree';
import { BehaviorSubject, merge, Observable } from 'rxjs';
import { CollectionViewer, SelectionChange } from '@angular/cdk/collections';
import { map } from 'rxjs/operators';
import { FileInfo } from '../contracts/file/file-info';

export class DynamicFlatNode {
  constructor(public item: FileInfo, public level = 1, public expandable = false,
              public isLoading = false) {}
}

export class DynamicDataSource {
  
  dataChange = new BehaviorSubject<DynamicFlatNode[]>([]);

  get data(): DynamicFlatNode[] { return this.dataChange.value; }
  set data(value: DynamicFlatNode[]) {
    this._treeControl.dataNodes = value;
    this.dataChange.next(value);
  }

  constructor(private _treeControl: FlatTreeControl<DynamicFlatNode>,
              private fs: FileService) {}

  connect(collectionViewer: CollectionViewer): Observable<DynamicFlatNode[]> {
    this._treeControl.expansionModel.onChange.subscribe(change => {
      if ((change as SelectionChange<DynamicFlatNode>).added ||
        (change as SelectionChange<DynamicFlatNode>).removed) {
        this.handleTreeControl(change as SelectionChange<DynamicFlatNode>);
      }
    });

    return merge(collectionViewer.viewChange, this.dataChange).pipe(map(() => this.data));
  }

  /** Handle expand/collapse behaviors */
  handleTreeControl(change: SelectionChange<DynamicFlatNode>) {
    if (change.added) {
      change.added.forEach(node => this.toggleNode(node, true));
    }
    if (change.removed) {
      change.removed.slice().reverse().forEach((node: DynamicFlatNode) => this.toggleNode(node, false));
    }
  }

  /**
   * Toggle the node, remove from display list
   */
  toggleNode(node: DynamicFlatNode, expand: boolean) {
    
    node.isLoading = true;

    this.fs.loadDirectory(node.item).subscribe(parent => {
      const children = parent.childNodes;
      const index = this.data.indexOf(node);
      if (!children || index < 0) { // If no children, or cannot find the node, no op
        return;
      }

      if (expand) {
        const nodes = children
          .sort(this.compareTreeNodes)
          .map(item => new DynamicFlatNode(item, node.level + 1, item.isExpandable));
          
        this.data.splice(index + 1, 0, ...nodes);
      } else {
        let count = 0;
        for (let i = index + 1; i < this.data.length
          && this.data[i].level > node.level; i++, count++) {}
        this.data.splice(index + 1, count);
      }

      // notify the change
      this.dataChange.next(this.data);
      node.isLoading = false;
    },
    error => {
      node.isLoading = false;
      console.error(error);
    });
  }

  /**
   * Compare two tree elements
   * @param a - first element
   * @param b - second element
   */
  private compareTreeNodes(a: FileInfo, b: FileInfo): number {
    if (a.isDirectory && !b.isDirectory) { //first, sorting by file type (directory on the top)
      return -1;
    } else if (!a.isDirectory && b.isDirectory) {
      return 1;
    } else {
      if (a.name.toUpperCase() > b.name.toUpperCase()) { //if both have same file, sort by name (asc)
        return 1;
      } else if (a.name.toUpperCase() < b.name.toUpperCase()) {
        return -1;
      }
      return 0;
    }
  }

  initialData() {
    this.fs.loadRootDirectory().subscribe(
      item => {
        this.data = [new DynamicFlatNode(item, 1, item.isExpandable)]
      },
      error => console.error(error));
  }
}