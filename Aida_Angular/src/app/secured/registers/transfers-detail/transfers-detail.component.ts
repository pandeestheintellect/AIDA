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
  selector: 'app-transfers-detail',
  templateUrl: './transfers-detail.component.html',
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class TransfersDetailComponent implements OnInit {

  action ='';
  localData:any;
  
  transferorList: DropDownModel[] = [
    {value: 'Male', text: '123:KAILASAM VIJAYA SELVI'},
    {value: 'Female', text: '124:Kumar'}
  ];
  transfereeList: DropDownModel[] = [
    {value: 'Male', text: '123:KAILASAM VIJAYA SELVI'},
    {value: 'Female', text: '124:Kumar'}
  ];

  
  constructor(public dialogRef: MatDialogRef<TransfersDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogOfficersDataModel,private snackBar: MatSnackBar,) {
      this.transferorList = JSON.parse(JSON.stringify(data.officers)) ;
      this.transferorList.shift();
      this.transfereeList = JSON.parse(JSON.stringify(data.officers)) ;
      this.transfereeList.shift();
      this.localData = JSON.parse(JSON.stringify(data.data)) ;
      this.localData.transferDate = moment(this.localData.transferDate,'DD/MM/YYYY');
      this.localData.transferorId = this.localData.transferorId+'';
      this.localData.transfereeId = this.localData.transfereeId+'';
      this.action = data.action;
     } 

  ngOnInit(): void {
    
  }

  OnOK(){
   
    if(this.localData.transferorId===this.localData.transfereeId)
    {
      this.OnShowMessage("Transferor and Transferee can not be same")
      return;
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
