import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { ResourceClientService } from '../../../service/resourceclient.service';

import { DropDownModel,DialogDataModel } from '../../../shared/models/common-data'



@Component({
  selector: 'app-service-definition-detail',
  templateUrl: './service-definition-detail.component.html',
  styles: []
})
export class ServiceDefinitionDetailComponent implements OnInit {

  action: string;
    localData:any;
    isModify:boolean;
    IsChecked:boolean=false;
  

  constructor(public dialogRef: MatDialogRef<ServiceDefinitionDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel,private resourceClient: ResourceClientService) 
  {
    this.localData = data.data;
    this.action = data.action;
    if (this.localData.hasOptionalDocument==='true')
    {
      this.IsChecked = true;
    }
    if (this.action==='Add')
        this.isModify=false;
      else
        this.isModify=true;
  }

  ngOnInit(): void {
    
  }

  OnOK(){

    if (this.IsChecked)
    {
      this.localData.hasOptionalDocument='true';
    }
    else
    {
      this.localData.hasOptionalDocument='';
    }

    this.dialogRef.close({event:this.action,data:this.localData});
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }


}
