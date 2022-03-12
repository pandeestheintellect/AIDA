import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { MatSnackBar} from '@angular/material/snack-bar';
import { ResourceClientService } from '../../../service/resourceclient.service';

import { DropDownModel,DialogDataModel } from '../../../shared/models/common-data'

import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';

import * as _moment from 'moment';

const moment = _moment;


@Component({
  selector: 'app-company-detail',
  templateUrl: './company-detail.component.html',
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class CompanyDetailComponent implements OnInit {

  action: string;
  localData:any;
  industryTypes: DropDownModel[] =[];
  activeStatus: DropDownModel[] = [
    {value: 'Active', text: 'Active'},
    {value: 'Terminated', text: 'Terminated'}
  ];

  constructor(public dialogRef: MatDialogRef<CompanyDetailComponent>,private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel,private resourceClient: ResourceClientService) 
  {
    this.localData = data.data;
    
    this.localData.incorpDate = moment(this.localData.incorpDate,'DD/MM/YYYY');
    this.localData.statusDate = moment(this.localData.statusDate,'DD/MM/YYYY');
    this.action = data.action;
  }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/masters/dropdown/industry-type').subscribe((data: DropDownModel[])=>{
      this.industryTypes = data;
    })  
  }
 
  OnOK(){
    if (isNaN(this.localData.fax))
    {
      this.OnShowMessage('Please enter a numeric value for Fax. ')
      return
    }
    if (this.localData.companyName.trim().length===0)
    {
      this.OnShowMessage('Please enter valid Company Name. ')
      return
    }
    if (this.localData.companyUEN.trim().length===0)
    {
      this.OnShowMessage('Please enter a valid Company UEN. ')
      return
    }
    this.dialogRef.close({event:this.action,data:this.localData});
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }

}
