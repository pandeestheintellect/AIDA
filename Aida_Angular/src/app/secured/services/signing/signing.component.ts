import { Component, OnInit, ViewChild,Inject } from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';

import { MatSnackBar} from '@angular/material/snack-bar';
import {MatTableDataSource} from '@angular/material/table'; 
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'

import { DialogDataModel,ResponseModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { ServiceRegistrationDisplayModel } from '../../../shared/models/service-data';
import { PreviewComponent } from 'src/app/shared/components/preview/preview.component';

export interface ServicesSignModel {
  serviceBusinessId:number;
  officerStepIds:string;
  
}

@Component({ 
  selector: 'app-signing',
  templateUrl: './signing.component.html',
  styles: []
})
export class SigningComponent implements OnInit {

  serviceBusinessId:number=0;
  officerId:number=0;

  displayedColumns: string[] = ['srNo','serviceName','businessProfileName','officerName','documentName','status','toolbox'];
  
  dataSource = new MatTableDataSource<ServiceRegistrationDisplayModel>();
  documents: ServiceRegistrationDisplayModel[] = [];
  

  constructor(public dialog: MatDialog,public dialogRef: MatDialogRef<SigningComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel,private resourceClient: ResourceClientService,private snackBar: MatSnackBar) { 
      this.serviceBusinessId = data.data.serviceBusinessId;
      this.officerId = data.data.officerId;
    }

  ngOnInit(): void {

     this.OnLoad();

  }
  OnLoad()
  {
    this.resourceClient.getDataInGet('api/service-registration/'+this.serviceBusinessId+'/'+this.officerId).subscribe((data: any[])=>{
      this.dataSource = new MatTableDataSource<ServiceRegistrationDisplayModel>(data); 
      this.documents = data;
      
    }) 
  }
  OnSignature(data)
  {
    
   if (data.generatedFileName==='NotGenerated' && data.status==='Signing')
   {
      var postdata:ServicesSignModel ={
        serviceBusinessId : this.serviceBusinessId,
        officerStepIds: data.officerStepId+''
      };
      
      this.resourceClient.getDataInPost('api/form-pdf/signing',postdata).subscribe((data: ResponseModel)=>{
        this.OnLoad();
        this.OnShowMessage(data.message);
      }) 
    
   }
   else if (data.status==='Completed')
   {
    this.OnShowMessage('Document already signed') ;
   }
   else if (data.status==='Terminated')
   {
    this.OnShowMessage('Document already terminated') ;
   }
   else if (data.status==='Signing Pending')
   {
    this.OnShowMessage('Document already sent for signing.') ;
   }
   else
    this.OnShowMessage('Document not ready for signing') ;
  }
  OnDownload(id,data)
  {
    if ((id===1 || id===2) && data.generatedFileName!=='NotGenerated')
    {
      var fileType="pdf";
    if (id===2)
      fileType="docx"; 
 
      window.open(this.resourceClient.REST_API_SERVER + 'api/file-download/'+data.generatedFileName,'_blank') ;
    }
    else if ( id===3 && data.downloadedFileName!=='NotDownloaded')
    {
      window.open(this.resourceClient.REST_API_SERVER + 'api/file-download/'+data.downloadedFileName,'_blank') ;
    }
    else
      this.OnShowMessage('Document not available now!.') ;
  }
  OnDownloadAll()
  {
    let noFile=0;
    for (let document of this.documents) {
      if(document.status==='Completed')
      {
        window.open(this.resourceClient.REST_API_SERVER + 'api/file-download/'+document.downloadedFileName,'_blank') ;
        noFile++
      }
    }

    if (noFile>0)
      this.OnShowMessage(noFile + ' Documents downloaded.') ;
    else
      this.OnShowMessage('Document not available now!.') ;
  }
  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }
  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }

}
