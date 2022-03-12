
import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'

import { ServiceSOPDetail } from '../../../shared/models/service-data';


import { DropDownModel } from '../../../shared/models/common-data';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { DocumentModel} from '../../../shared/models/document-data';

@Component({
  selector: 'app-services-sop-detail',
  templateUrl: './services-sop-detail.component.html',
  styleUrls: ['./services-sop-detail.component.scss']
})

export class ServicesSOPDetailComponent implements OnInit {

  action: string;
  userRole:string;
  serviceCode:string;

  isModify:boolean;
  userRoleList: DropDownModel[] = [
    {value: 'Authorised Representative', text: 'Authorised Representative'},
    {value: 'Director', text: 'Director'},
    {value: 'Secretary', text: 'Secretary'},
    {value: 'Shareholder', text: 'Shareholder'},
    {value:'Partner',text:'Partner'},
    {value:'Precedent Partner',text:'Precedent Partner'},
    {value:'Manager',text:'Manager'},
    {value:'Sole Proprietor',text:'Sole Proprietor'}
  ];

    fulllistDocument :DocumentModel[] = [];
    availableDocument :DocumentModel[] = [];
    selectedDocument :DocumentModel[] = [];

    constructor(public dialogRef: MatDialogRef<ServicesSOPDetailComponent>,
      @Inject(MAT_DIALOG_DATA) public data: ServiceSOPDetail,private resourceClient: ResourceClientService) 
    {
      this.userRole = data.executor;
      this.action = data.action;
      this.serviceCode = data.serviceCode;
      
      if (this.action==='Add')
        this.isModify=false;
      else
        this.isModify=true;
    }
  
    ngOnInit(): void {
      this.resourceClient.getDataInGet('api/service-sop'+'/'+this.serviceCode+'/'+this.userRole).subscribe((data: DocumentModel[])=>{
        this.availableDocument = [];
        this.selectedDocument= [];
        for (var doc of data) 
        {  
          if (doc.status==='')
            this.availableDocument.push(doc);
          else
            this.selectedDocument.push(doc);
        }  
      }) 
    }
    OnGo()
    {
      this.resourceClient.getDataInGet('api/documents').subscribe((data: DocumentModel[])=>{
        this.availableDocument = data;
        this.fulllistDocument = data;
      }) 
    }
    OnOK(){
      this.dialogRef.close({event:this.action,data:{executor:this.userRole,document:this.selectedDocument}});
    }
  
    OnClose(){
      this.dialogRef.close({event:'Cancel'});
    }
  
    drop(event: CdkDragDrop<DocumentModel[]>) {
      if (event.previousContainer === event.container) {
        moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
      } else {
        transferArrayItem(event.previousContainer.data,
                          event.container.data,
                          event.previousIndex,
                          event.currentIndex);
      }
    }

  }
  