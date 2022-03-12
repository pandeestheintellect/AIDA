import { Component, OnInit,Inject } from '@angular/core';

import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { MatSnackBar} from '@angular/material/snack-bar';
import { ResourceClientService } from 'src/app/service/resourceclient.service';
import { DropDownModel,DialogOfficersDataModel } from '../../../shared/models/common-data'

@Component({
  selector: 'app-nominee-nominators-detail',
  templateUrl: './nominee-nominators-detail.component.html',
  styles: []
})
export class NomineeNominatorsDetailComponent implements OnInit {

  action ='';
  localData:any;
  memberList: DropDownModel[] = [];
  nominatorMasterList: DropDownModel[] = [];
  nominatorList: DropDownModel[] = [];
  natureList: DropDownModel[] = [
    {value: 'Individual', text: 'Individual'},
    {value: 'Corporate', text: 'Corporate'}
  ];
  
  constructor(public dialogRef: MatDialogRef<NomineeNominatorsDetailComponent>,private resourceClient: ResourceClientService,
    @Inject(MAT_DIALOG_DATA) public data: DialogOfficersDataModel,private snackBar: MatSnackBar,) {
      this.memberList = JSON.parse(JSON.stringify(data.officers)) ;
      this.memberList.shift();
      this.localData = JSON.parse(JSON.stringify(data.data)) ;

      this.localData.officerId = this.localData.officerId+'';
      this.action = data.action;
      
     } 

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/register/nominator-dropdown/'+this.localData.businessProfileId).subscribe((data: DropDownModel[])=>{
      this.nominatorMasterList = data;
      this.onNatureChange();
    });
  }

  onNatureChange()
  {
    var type: DropDownModel[] =[];
    type = this.nominatorMasterList.filter((nature)=>nature.value.charAt(0)===this.localData.nature.charAt(0))
    this.nominatorList = type;
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
