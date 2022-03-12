import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { ResourceClientService } from '../../../service/resourceclient.service';

import { DropDownModel,DialogDataModel } from '../../../shared/models/common-data'
import { EntityShareholderModel } from '../../../shared/models/client-profile-data';
import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';

import * as _moment from 'moment';
import { ImportDocumentsComponent } from '../import-documents/import-documents.component';

const moment = _moment;

@Component({
  selector: 'app-entity-shareholder-details',
  templateUrl: './entity-shareholder-details.component.html',
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})

export class EntityShareholderDetailsComponent implements OnInit {

  action: string;
  localData:any;
  businessProfileId=0;
  representativeList: DropDownModel[] =[];
  activeStatus: DropDownModel[] = [
    {value: 'Active', text: 'Active'},
    {value: 'Terminated', text: 'Terminated'}
  ];

  constructor(private dialog: MatDialog,public dialogRef: MatDialogRef<EntityShareholderModel>,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel,private resourceClient: ResourceClientService) 
  {
    this.localData = data.data;
    this.localData.incorpDate = moment(this.localData.incorpDate,'DD/MM/YYYY');
    this.businessProfileId = data.data.businessProfileId;
    this.action = data.action;
  }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/business-profile-representative/'+this.localData.businessProfileId).subscribe((data: DropDownModel[])=>{
      this.representativeList = data;
    })  
  }

  OnOK(){
    this.dialogRef.close({event:this.action,data:this.localData});
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }
  

  OnImport()
  {
    const dialogRef = this.dialog.open(ImportDocumentsComponent, {
      width: '600px',
      data:{action:'Import',data:'Entity-Shareholder'},
      disableClose: true  
    });

    dialogRef.afterClosed().subscribe(result => 
    {
      if (result)
      {
        if(result.event==='Upload')
        {
          this.resourceClient.getDataInGet('api/import-entity/'+btoa(result.data)).subscribe((data: EntityShareholderModel)=>{
            this.localData = data;
            this.localData.businessProfileId =this.businessProfileId ;
          });
 
        }
      }
    });
  }

}
