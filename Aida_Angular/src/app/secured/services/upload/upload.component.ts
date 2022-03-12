import { Component, OnInit, Output, EventEmitter , ViewChild,Inject} from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
 

import { MatSnackBar} from '@angular/material/snack-bar';
import {MatTableDataSource} from '@angular/material/table'; 
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'


import { ResourceClientService } from '../../../service/resourceclient.service';
import { ServiceRegistrationDisplayModel } from '../../../shared/models/service-data';
import { PreviewComponent } from 'src/app/shared/components/preview/preview.component';
import { DropDownModel,DialogDataModel } from '../../../shared/models/common-data'

@Component({
  selector: 'app-upload', 
  templateUrl: './upload.component.html',
  styles: []
})
export class UploadComponent implements OnInit {

  public progress: number;
  public message: string;
  @Output() public onUploadFinished = new EventEmitter();
 
  //path="http://localhost:63644/api/file-upload/";
  path="api/file-upload/";
  serviceBusinessId:number=0;
  officerId:number=0;
  serviceCode='';
  fileType:string='';
  fileList: DropDownModel[] = [
    {value: 'Passport', text: 'Passport'},
    {value: 'NRIC', text: 'NRIC'},
    {value: 'FIN', text: 'FIN'},
    {value: 'Statutory Register', text: 'Statutory Register'},
    {value: 'Other', text: 'Other'}
  ];

  constructor(public dialog: MatDialog,public dialogRef: MatDialogRef<UploadComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel,private resourceClient: ResourceClientService,
    private snackBar: MatSnackBar,private http: HttpClient) { 
      this.serviceBusinessId = data.data.serviceBusinessId;
      this.officerId = data.data.officerId;
      this.serviceCode = data.data.serviceName;
    }

  ngOnInit(): void { 
  }

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }
  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
 
    let fileToUpload = <File>files[0];
    if (fileToUpload.type==='image/jpeg' ||fileToUpload.type==='application/pdf')
    {
      const formData = new FormData();
      formData.append('file', fileToUpload, fileToUpload.name);
   
      this.http.post(this.resourceClient.REST_API_SERVER + this.path + this.serviceBusinessId +'/'+ this.officerId+'/'+ this.serviceCode+'/'+this.fileType, formData, {reportProgress: true, observe: 'events'})
        .subscribe(event => {
          if (event.type === HttpEventType.UploadProgress)
            this.progress = Math.round(100 * event.loaded / event.total);
          else if (event.type === HttpEventType.Response) {
            this.message = 'Upload success.';
            this.onUploadFinished.emit(event.body);
          }
        });
    }
    else
    {
      this.OnShowMessage ("Only JPG & PDF files are allowed.") 
    }
  }
  
}
