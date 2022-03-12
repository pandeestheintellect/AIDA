import { Component, OnInit , Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

import { ResourceClientService } from '../../../service/resourceclient.service';
import {DocPreviewModel} from '../../models/common-data';

@Component({
  selector: 'app-webframe',
  template: `

<div class="app-dialog" style="width:100%;height:100%;">

<div class="page-sub-header" style="position:absolute;top:10px;z-index:2;float:right;" > 
  <button mat-button (click)="OnClose()" mat-flat-button color="warn" style="text-align:right;margin-top: 10px;">Close</button>
</div>
<div style="width: 100%;height:100%">
  <iframe style="width: 100%;height:100%" frameBorder="0" [src]="urlSafe" ></iframe>
</div>


</div>	
    
  `,
  styles: [
  ]
})
export class WebframeComponent implements OnInit {

  serviceBusinessId=0;
  officerStepId=0;
  url: string = "api/http-download/UHJldjc1NzI4Njk1LTEtMS1DRERLWUNGb3Jt";
  urlSafe: SafeResourceUrl;

  constructor(public dialogRef: MatDialogRef<WebframeComponent>,@Inject(MAT_DIALOG_DATA) public data: DocPreviewModel,
    private resourceClient: ResourceClientService,public sanitizer: DomSanitizer) { 
      this.serviceBusinessId = data.serviceBusinessId;
      this.officerStepId = data.officerStepId;
    }

  ngOnInit(): void {
    this.urlSafe= this.sanitizer.bypassSecurityTrustResourceUrl(this.resourceClient.REST_API_SERVER+ '/api/form-filling/'+this.serviceBusinessId+'/'
    +this.officerStepId);

  }

  OnClose()
  {
    this.dialogRef.close();
  }
}
