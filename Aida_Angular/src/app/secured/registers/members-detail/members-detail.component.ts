import { Component, OnInit,Inject } from '@angular/core';

import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { MatSnackBar} from '@angular/material/snack-bar';
import { DropDownModel,DialogOfficersDataModel } from '../../../shared/models/common-data'

import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';

import * as _moment from 'moment';

const moment = _moment;


@Component({
  selector: 'app-members-detail',
  templateUrl: './members-detail.component.html',
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class MembersDetailComponent implements OnInit {

  action ='';
  localData:any;
  title = 'Members';
    identityTypeList: DropDownModel[] = [];
  memberList: DropDownModel[] = [
    {value: 'Male', text: '123:KAILASAM VIJAYA SELVI'},
    {value: 'Female', text: '124:Kumar'}
  ];
  
  constructor(public dialogRef: MatDialogRef<MembersDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogOfficersDataModel,private snackBar: MatSnackBar,) {
      this.memberList = JSON.parse(JSON.stringify(data.officers)) ;
      this.memberList.shift();
      this.localData = JSON.parse(JSON.stringify(data.data)) ;
      this.localData.entryDate = moment(this.localData.entryDate,'DD/MM/YYYY');
      this.localData.allotmentDate = moment(this.localData.allotmentDate,'DD/MM/YYYY');
      this.localData.officerId = this.localData.officerId+'';
      this.action = data.action;
      this.identityTypeList = data.identityTypes;
      if (this.localData.userRole)
      {
        this.title = this.localData.userRole
      }
     } 

  ngOnInit(): void {
    
  }

  OnOK(){
   
    
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