import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import {MatTableDataSource} from '@angular/material/table';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { DocumentFieldModel } from '../../../shared/models/document-data';
import { DropDownModel,DialogDataModel } from '../../../shared/models/common-data'

@Component({
  selector: 'app-document-detail',
  templateUrl: './document-detail.component.html'
})
export class DocumentDetailComponent implements OnInit {

  action: string;
  localData:any;
  statusList: DropDownModel[] = [
    {value: 'Active', text: 'Active'},
    {value: 'Suspended', text: 'Suspended'}
  ];
 
  displayedColumns: string[] = ['keyword','label','control','nature','isRequired'];
  
  dataSource = new MatTableDataSource<DocumentFieldModel>();

  constructor(public dialogRef: MatDialogRef<DocumentDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel,private resourceClient: ResourceClientService) 
  {
    this.localData = data.data;
    this.localData.effectiveDate = new Date(this.localData.effectiveDate);
    this.action = data.action;
  }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/documents/'+this.localData.code).subscribe((data: DocumentFieldModel[])=>{
      this.dataSource = new MatTableDataSource<DocumentFieldModel>(data);
    })
  }

  OnOK(){
    this.dialogRef.close({event:this.action,data:this.localData});
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }


}
