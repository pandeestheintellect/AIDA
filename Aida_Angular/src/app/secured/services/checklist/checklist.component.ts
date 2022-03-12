
import { Component, OnInit, ViewChild,Inject } from '@angular/core';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';

import { ResourceClientService } from '../../../service/resourceclient.service';

import { AngularTreeGridComponent } from 'angular-tree-grid';
import { documentsData } from './documents-data';

@Component({
  selector: 'app-download-forms',
  template: `
    
    <div class="mat-elevation-z4 app-page">

    <mat-toolbar class="page-toolbar">
        <h2 class="mat-headline center-align" style="margin-left: 20px;">{{title}}</h2>
        <span class="header-spacer"></span>
        <div fxLayout="row" fxLayoutGap="10px" *ngIf="hasDocument">
    
          <button mat-button style="height: 40px !important;" (click)="expandAll()"    mat-flat-button color="primary">Expand All</button>
          <button mat-button style="height: 40px !important;" (click)="collapseAll()" mat-flat-button color="primary">Collapse All</button>
          <button mat-button style="height: 40px !important;" (click)="downloadAll()" mat-flat-button color="primary">Download</button>
      </div> 
    </mat-toolbar>
    <div class="page-section" *ngIf="hasDocument">
      <db-angular-tree-grid #angularGrid [data]="gridData" [configs]="configs"></db-angular-tree-grid>
    </div>
    
        
    </div>  

  `,
  styles: []
})
export class ChecklistComponent implements OnInit {

  title='';
  rootPath="api/service-sop";
  serviceCode:string='';
  hasDocument:boolean=true;
  
  @ViewChild('angularGrid') angularGrid: AngularTreeGridComponent;
  documentsData = documentsData;
  gridData: any = [];

  configs: any = {
    id_field: 'id',
    parent_id_field: 'parent',
    parent_display_field: 'service',
    multi_select: true,
    css: { // Optional
      expand_class: 'fa fa-caret-right',
      collapse_class: 'fa fa-caret-down'
    },
    columns: [
      {
        name: 'service',
        header: 'Service'
      },
      {
        name: 'documentName',
        header: 'Document'
      }
    ]
  };

  constructor(
    private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private snackBar: MatSnackBar)
    { 
      
    } 

  ngOnInit(): void {

    this.route.params.subscribe(params => {
      var id = -1;
      var serviceCode = params['serviceCode']

      if (serviceCode==='registrations')
        id=0;
      else if (serviceCode==='registration-sole-proprietor')
        id=1;
      else if (serviceCode==='appointment')
        id=2;
      else if (serviceCode==='ec-clients')
        id=3;
      else if (serviceCode==='nc-clients')
        id=4;
      else if (serviceCode==='internals')
        id=5;

      if (id>=0)
      {
        this.title='Download ' + this.documentsData[id].service + ' forms' 

        this.gridData = this.documentsData[id].documents;
 
      }
      else
      {
        this.title='Download forms not available for this service' ;
        this.hasDocument=false;
      }  

    });
       
    
  }

  downloadAll(){

    var currentParent=-1;
    let noFile=0;
    var documents:string[]=[];

    this.gridData.forEach(model => {
      
      if('row_selected' in model){
        if (model.leaf===false && model.row_selected===true)
          currentParent=model.id ;
        else if (model.row_selected===true)
        {
          if (documents.indexOf(btoa(model.fileName)) === -1)
            documents.push (btoa(model.fileName)) ;
        }
      }
      else
      {
        if(currentParent===model.parent)
        {
          if (documents.indexOf(btoa(model.fileName)) === -1)
            documents.push (btoa(model.fileName)) ;
        }
          
      } 

    }); 
    
    for (let document of documents) {
        window.open(this.resourceClient.REST_API_SERVER + 'api/file-upload-download/'+document,'_blank') ;
        noFile++
    }

    if (noFile>0)
      this.OnShowMessage(noFile + ' Documents downloaded.') ;
    else
      this.OnShowMessage('Document not downloaded.') ;
  } 

  

  expandAll() {
    this.angularGrid.expandAll();
  }
  collapseAll() {
    this.angularGrid.collapseAll();
  }

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }
}
