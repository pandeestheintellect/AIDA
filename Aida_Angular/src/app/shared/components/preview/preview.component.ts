import { Component, OnInit , Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'

import { ResourceClientService } from '../../../service/resourceclient.service';
import {DocPreviewModel} from '../../models/common-data';

@Component({
  selector: 'app-preview',
  template: `

<div class="app-dialog" style="width:100%;height:100%;">

    <div class="page-sub-header" style="position:absolute;top:10px;z-index:2;float:right;" > 
      <button mat-button (click)="OnClose()" mat-flat-button color="warn" style="text-align:right;margin-top: 10px;">Close</button>
    </div>
    <div class="pdf-container">
      <pdf-viewer [src]="doc"
                  [original-size]="false"
      ></pdf-viewer>
    </div>

    
</div>	

  `,
  styles: []
})
export class PreviewComponent implements OnInit {

  viewer = 'google';
  doc = '';

  rootPath='api/service-execution';
  

  documents:string;
  fileName='';
  serviceBusinessId=0;
  officerStepId=0;

  constructor(public dialogRef: MatDialogRef<PreviewComponent>,@Inject(MAT_DIALOG_DATA) public data: DocPreviewModel,private resourceClient: ResourceClientService) {
    this.serviceBusinessId = data.serviceBusinessId;
    this.officerStepId = data.officerStepId;
    this.fileName = data.fileName;
   }

  ngOnInit(): void {

    if (this.fileName !==undefined)
    {
      this.doc = this.resourceClient.REST_API_SERVER + 'api/file-preview/'+this.fileName;
    }
    else
    {
      this.doc = this.resourceClient.REST_API_SERVER + 'api/file-preview/bG9hZGluZy5wZGY=';
      this.resourceClient.getDataInGet('api/form-preview-pdf/'+this.serviceBusinessId+'/'
      +this.officerStepId).subscribe((data: any)=>{
        this.fileName = data;
        this.doc = this.resourceClient.REST_API_SERVER + 'api/file-preview/'+data;
        
      });

    }

  }

  OnClose()
  {
    this.dialogRef.close();
  }
}
