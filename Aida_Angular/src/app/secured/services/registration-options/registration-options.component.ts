import { Component, OnInit, ViewChild,Inject } from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';

import { ResponseModel,DropDownModel,DialogDataModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { ServiceRegistrationDisplayModel,ServicesDefinitionModel } from '../../../shared/models/service-data';
import { DocumentModel } from '../../../shared/models/document-data';
import {SelectionModel} from '@angular/cdk/collections';
import { ServicesSOPModel } from '../../../shared/models/service-data';

import { AngularTreeGridComponent } from 'angular-tree-grid';


@Component({
  selector: 'app-registration-options',
  templateUrl: './registration-options.component.html',
  styles: []
})
export class RegistrationOptionsComponent implements OnInit {

  rootPath="api/service-sop";
  serviceCode:string='';

  displayedColumns: string[] = ['select','filePath','documentName','executor','versionNo'];
  
  dataSource = new MatTableDataSource<ServicesSOPModel>();
  selection = new SelectionModel<ServicesSOPModel>(true, []);


  @ViewChild('angularGrid') angularGrid: AngularTreeGridComponent;

  gridData: any = [];

  configs: any = {
    id_field: 'Id',
    parent_id_field: 'parent',
    parent_display_field: 'filePath',
    multi_select: true,
    css: { // Optional
      expand_class: 'fa fa-caret-right',
      collapse_class: 'fa fa-caret-down',
    },
    columns: [
      {
        name: 'filePath',
        header: 'Service'
      },
      {
        name: 'documentName',
        header: 'Document'
      },
      {
        name: 'executor',
        header: 'Execution By'
       
      },
      {
        name: 'versionNo',
        header: 'Document Version'
      }
    ]
  };


  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  constructor(public dialogRef: MatDialogRef<RegistrationOptionsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string,private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private snackBar: MatSnackBar)
    { 
      this.serviceCode=data;
    } 

  ngOnInit(): void {

    
    this.resourceClient.getDataInGet(this.rootPath+'/'+this.serviceCode).subscribe((data: any[])=>{
      this.dataSource = new MatTableDataSource<ServicesSOPModel>(data);

      let grid = [];
      let parentid=0;
      let childid=1;
      let oldPath='';
      for (let i=0;i<data.length;i++)
      {
        if (oldPath!==data[i].filePath)
        {
          oldPath=data[i].filePath;
          grid.push({ Id: childid, stepId:0,  documentName: '', executor:'' , versionNo: '', filePath: data[i].filePath, parent: 0});
          parentid=childid;
          childid++;
        }
        grid.push({ Id: childid,stepId: data[i].stepId, documentName: data[i].documentName, executor:data[i].executor , versionNo: data[i].versionNo, filePath:'', parent: parentid});
        childid++;
      }
      this.gridData = grid;

    })   
    
    
  }

  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.dataSource.data.forEach(row => this.selection.select(row));
  }

  OnOK(){

    var stepIds:string[]=[];
    var currentParent=-1;

    this.gridData.forEach(model => {
      
      if('row_selected' in model){
        if (model.leaf===false)
          currentParent=model.Id ;
        else if(parseInt(model.stepId)>0)
          stepIds.push(model.stepId) ;
      }
      else
      {
        if(parseInt(model.stepId)>0 && currentParent===model.parent)
          stepIds.push(model.stepId) ;
          
      }

    }); 
    
    if (stepIds.length>0)
    this.dialogRef.close({event:'Update',data:stepIds.join(',')});
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }

  expandAll() {
    this.angularGrid.expandAll();
  }
  collapseAll() {
    this.angularGrid.collapseAll();
  }

}
