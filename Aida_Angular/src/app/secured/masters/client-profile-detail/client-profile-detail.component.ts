import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { ResourceClientService } from '../../../service/resourceclient.service';

import { DropDownModel,DialogDataModel } from '../../../shared/models/common-data'
import { ClientProfileModel } from '../../../shared/models/client-profile-data';
import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';

import * as _moment from 'moment';
import { ImportDocumentsComponent } from '../import-documents/import-documents.component';
import { FormaterService } from 'src/app/service/formater.service';

const moment = _moment;

@Component({
  selector: 'app-client-profile-detail',
  templateUrl: './client-profile-detail.component.html',
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class ClientProfileDetailComponent implements OnInit {

  action: string;
  localData:any;
  industryTypes: DropDownModel[] =[];
  activeStatus: DropDownModel[] = [];

  identityType: DropDownModel[] = [
    {value: 'NRIC', text: 'NRIC'},
    {value: 'FIN', text: 'FIN'},
    {value: 'Passport', text: 'Passport'},
    {value: 'Others', text: 'Others'}

  ];
  clientTypes: DropDownModel[] = [
    {value: 'PRIVATE LIMITED COMPANY', text: 'PRIVATE LIMITED COMPANY'},
    {value: 'PUBLIC LIMITED COMPANY', text: 'PUBLIC LIMITED COMPANY'},
    {value: 'SOLE PROPRIETORSHIP', text: 'SOLE PROPRIETORSHIP'},
    {value: 'PARTNERSHIP', text: 'PARTNERSHIP'},
    {value: 'LLP', text: 'LLP'},
    {value: 'LP', text: 'LP'},
    {value: 'VCC', text: 'VCC'}
  ];

  constructor(private dialog: MatDialog,public dialogRef: MatDialogRef<ClientProfileDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel,private resourceClient: ResourceClientService,public formater:FormaterService) 
  {
    this.localData = data.data;
    this.localData.incorpDate = moment(this.localData.incorpDate,'DD/MM/YYYY');
    this.localData.statusDate = moment(this.localData.statusDate,'DD/MM/YYYY');
    this.action = data.action;

    this.localData.issuedCapital =this.formater.getFormatedCurrencyValue(this.localData.issuedCapital);
    this.localData.issuedShares =this.formater.getFormatedCurrencyValue(this.localData.issuedShares);
    this.localData.paidupCapital =this.formater.getFormatedCurrencyValue(this.localData.paidupCapital);
    this.localData.paidupShares =this.formater.getFormatedCurrencyValue(this.localData.paidupShares);
  }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/masters/dropdown/industrial-classification').subscribe((data: DropDownModel[])=>{
      this.industryTypes = data;
    })  

    this.resourceClient.getDataInGet('api/masters/dropdown/client-status').subscribe((data: DropDownModel[])=>{
      this.activeStatus = data;
    })  

  }
  transformAmount(element)
  {
    element.target.value= this.formater.getFormatedCurrencyValue(element.target.value);
  }
  getNumbers(numberString,isFlot)
  {
    let num=numberString+'';
    //numberString = num.replaceAll(',','')
    numberString=(numberString+'').replace(/,/g, '') ;
    if (numberString.trim().length >0)
    {
      if(isFlot===true)
        return  parseFloat(numberString);
      else
        return parseInt(numberString);
    }
    else
      return 0;
  }

  OnOK(){

    if(this.action==='Update')
    {
      this.localData.issuedCapital =this.getNumbers(this.localData.issuedCapital,true);
      this.localData.issuedShares =this.getNumbers(this.localData.issuedShares,false);
      this.localData.paidupCapital =this.getNumbers(this.localData.paidupCapital,true);
      this.localData.paidupShares =this.getNumbers(this.localData.paidupShares,false);
  
    }


    this.dialogRef.close({event:this.action,data:this.localData});
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }
  

  OnImport()
  {
    const dialogRef = this.dialog.open(ImportDocumentsComponent, {
      width: '600px',
      data:{action:'Import',data:'Client-Profile'},
      disableClose: true  
    });

    dialogRef.afterClosed().subscribe(result => 
    {
      if (result)
      {
        if(result.event==='Upload')
        {
          this.resourceClient.getDataInGet('api/import-client/'+btoa(result.data)).subscribe((data: ClientProfileModel)=>{
            this.localData = data;
          });
 
        }
      }
    });
  }

}
