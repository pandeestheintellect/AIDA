import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { ResourceClientService } from '../../../service/resourceclient.service';
import { ImportDocumentsComponent } from '../import-documents/import-documents.component';
import { DropDownModel,DialogDataModel } from '../../../shared/models/common-data'
import { ClientOfficerModel } from '../../../shared/models/client-officer-data';
import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';

import { FormaterService } from '../../../service/formater.service';

import * as _moment from 'moment';

const moment = _moment;

 

@Component({
  selector: 'app-client-officer-detail',
  templateUrl: './client-officer-detail.component.html',
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class ClientOfficerDetailComponent implements OnInit {

  action: string;
  localData:any;
  userRole: DropDownModel[] =[];
  roleType: DropDownModel[] =[];
  clientProfileType='';
  myInfo=false;
  userSex: DropDownModel[] = [
    {value: 'MALE', text: 'MALE'},
    {value: 'FEMALE', text: 'FEMALE'}
  ];
  businessProfileId=0;
  constructor(private dialog: MatDialog,public dialogRef: MatDialogRef<ClientOfficerDetailComponent>,private resourceClient: ResourceClientService,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel,public formater:FormaterService) 
  {
    this.localData = data.data;
    this.action = data.action.split('~')[0];
    this.clientProfileType =data.action.split('~')[1]; 
    this.localData.birthDate = moment(this.localData.birthDate,'DD/MM/YYYY');
    this.localData.nricIssueDate = moment(this.localData.nricIssueDate,'DD/MM/YYYY');
    this.localData.nricExpiryDate = moment(this.localData.nricExpiryDate,'DD/MM/YYYY');
    this.localData.finIssueDate = moment(this.localData.finIssueDate,'DD/MM/YYYY');
    this.localData.finExpiryDate = moment(this.localData.finExpiryDate,'DD/MM/YYYY');
    this.localData.passportIssueDate = moment(this.localData.passportIssueDate,'DD/MM/YYYY');
    this.localData.passportExpiryDate = moment(this.localData.passportExpiryDate,'DD/MM/YYYY');
    this.localData.joinDate = moment(this.localData.joinDate,'DD/MM/YYYY');

    this.localData.numberOfShares =this.formater.getFormatedCurrencyValue(this.localData.numberOfShares);
    this.businessProfileId = this.localData.businessProfileId;
    if (this.localData.myInfoRequestId>0)
      this.myInfo=true;
    this.OnLoadRole();
    this.OnRoleChange();
    
  }

  ngOnInit(): void {
    
  }
  transformAmount(element)
  {
    element.target.value= this.formater.getFormatedCurrencyValue(element.target.value);
  }
  OnOK(){
    this.localData.numberOfShares = (this.localData.numberOfShares+'').replace(/,/g, '') 

    if (this.localData.numberOfShares.trim().length >0)
    this.localData.numberOfShares = parseInt(this.localData.numberOfShares);
    else
    this.localData.numberOfShares=0;

    this.dialogRef.close({event:this.action,data:this.localData});
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }

  OnLoadRole()
  {
    var type: DropDownModel[] =[];
    
    if(this.localData.userRole==='Authorised Representative')
      type.push({value: 'Authorised Representative', text: 'Authorised Representative'}); 
    else
    {
      if (this.clientProfileType==='PARTNERSHIP')
      {
        type.push({value:'Partner',text:'Partner'}) 
        type.push({value:'Precedent Partner',text:'Precedent Partner'})
        type.push({value:'Manager',text:'Manager'})
      }
      else if (this.clientProfileType==='LLP' || this.clientProfileType==='LP')
      {
        type.push({value:'Partner',text:'Partner'}) 
        type.push({value:'Precedent Partner',text:'Precedent Partner'})
      }
      else if (this.clientProfileType==='SOLE PROPRIETORSHIP')
      {
        type.push({value:'Sole Proprietor',text:'Sole Proprietor'}) 
        type.push({value:'Manager',text:'Manager'})
      }
      else
      {
        type.push({value: 'Director', text: 'Director'});
        type.push({value: 'Shareholder', text: 'Shareholder'});
        type.push({value: 'Secretary', text: 'Secretary'});
        type.push({value: 'Chairman', text: 'Chairman'});
        type.push({value: 'Auditor', text: 'Auditor'});
        
      }
  
    }
    
    this.userRole = type;
  }

  OnRoleChange()
  {
    var type: DropDownModel[] =[];
    if (this.localData.userRole==='Director')
    {
      type.push({value:'',text:''})
      type.push({value:'Director Shareholder',text:'Director Shareholder'})
    }
    else if (this.localData.userRole==='Shareholder')
    {
      type.push({value:'Individual',text:'Individual'})
      type.push({value:'Entity', text: 'Entity'})
      type.push({value:'U.Beneficial Owner',text:'U.Beneficial Owner'})
      type.push({value:'Beneficial Owner', text: 'Beneficial Owner'})
      if (this.localData.entityType==='')
        this.localData.entityType='Individual';
    }
    this.roleType = type;
  }

  OnImport()
  {
    const dialogRef = this.dialog.open(ImportDocumentsComponent, {
      width: '600px',
      data:{action:'Import',data:'Officer-Profile'},
      disableClose: true  
    });

    dialogRef.afterClosed().subscribe(result => 
    {
      if (result)
      {
        if(result.event==='Upload')
        {
          this.resourceClient.getDataInGet('api/import-officer/'+btoa(result.data)).subscribe((data: ClientOfficerModel)=>{
            this.localData = data;

            this.localData.birthDate = moment(this.localData.birthDate,'DD/MM/YYYY');
            this.localData.nricIssueDate = moment(this.localData.nricIssueDate,'DD/MM/YYYY');
            this.localData.nricExpiryDate = moment(this.localData.nricExpiryDate,'DD/MM/YYYY');
            this.localData.finIssueDate = moment(this.localData.finIssueDate,'DD/MM/YYYY');
            this.localData.finExpiryDate = moment(this.localData.finExpiryDate,'DD/MM/YYYY');
            this.localData.passportIssueDate = moment(this.localData.passportIssueDate,'DD/MM/YYYY');
            this.localData.passportExpiryDate = moment(this.localData.passportExpiryDate,'DD/MM/YYYY');
            this.localData.joinDate = moment(this.localData.joinDate,'DD/MM/YYYY');

            this.localData.numberOfShares =this.formater.getFormatedCurrencyValue(this.localData.numberOfShares);
            this.localData.businessProfileId = this.businessProfileId ;
            
          });
 
        }
      }
    });
  }
}
