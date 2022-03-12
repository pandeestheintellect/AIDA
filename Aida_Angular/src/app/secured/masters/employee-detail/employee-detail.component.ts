import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { ResourceClientService } from '../../../service/resourceclient.service';
import { MatSnackBar} from '@angular/material/snack-bar';
import { DropDownModel,DialogDataModel } from '../../../shared/models/common-data'

import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';

import * as _moment from 'moment';

const moment = _moment;




@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class EmployeeDetailComponent implements OnInit {

  action: string;
  localData:any;
  
  genderList: DropDownModel[] = [
    {value: 'Male', text: 'Male'},
    {value: 'Female', text: 'Female'}
  ];

  positionList: DropDownModel[] = [];
  roleList: DropDownModel[] = [];
  
  positionListSelected: DropDownModel[] = [];
  roleListSelected: DropDownModel[] = [];

  identityType: DropDownModel[] = [
    {value: 'NRIC', text: 'NRIC'},
    {value: 'FIN', text: 'FIN'},
    {value: 'Passport', text: 'Passport'},
    {value: 'Others', text: 'Others'}

  ];

  loginRoleList: DropDownModel[] = [
    {value: 'None', text: 'None'},
    {value: 'Operator', text: 'Operator'},
    {value: 'Admin', text: 'Admin'}
  ];

  compareFunction = (o1: DropDownModel, o2: DropDownModel)=> o1.value===o2.value;

  constructor(public dialogRef: MatDialogRef<EmployeeDetailComponent>,private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel,private resourceClient: ResourceClientService) 
  {
    this.localData = data.data;
    this.localData.birthDate = moment(this.localData.birthDate,'DD/MM/YYYY');
    this.action = data.action;
  }

  ngOnInit(): void {
    
    this.resourceClient.getDataInGet('api/masters/dropdown/positions').subscribe((data: DropDownModel[])=>{
      this.positionList = data;
    })
    this.resourceClient.getDataInGet('api/masters/dropdown/roles').subscribe((data: DropDownModel[])=>{
      this.roleList = data;
    })
    
    this.resourceClient.getDataInGet('api/employees/'+this.localData.code+'/position').subscribe((data: DropDownModel[])=>{
      this.positionListSelected = data;
    })
    this.resourceClient.getDataInGet('api/employees/'+this.localData.code+'/role').subscribe((data: DropDownModel[])=>{
      this.roleListSelected = data;
    })
  }

  OnOK(){
    if (this.localData.firstName.trim().length===0)
    {
      this.OnShowMessage('Please enter a valide name. ')
      return
    }
    this.dialogRef.close({event:this.action,data:{master:this.localData,position:this.positionListSelected,role:this.roleListSelected}});
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }
  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }

}
