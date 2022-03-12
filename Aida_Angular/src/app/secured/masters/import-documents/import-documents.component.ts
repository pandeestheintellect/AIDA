import { Component, OnInit, Output, EventEmitter , ViewChild,Inject} from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
 

import { MatSnackBar} from '@angular/material/snack-bar';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'


import { ResourceClientService } from '../../../service/resourceclient.service';
import { DropDownModel,DialogDataModel } from '../../../shared/models/common-data'

@Component({
  selector: 'app-import-documents',
  templateUrl: './import-documents.component.html',
  styles: []
})
export class ImportDocumentsComponent implements OnInit {

  public progress: number;
  public message: string;
  @Output() public onUploadFinished = new EventEmitter();
 
  //path="http://localhost:63644/api/file-upload/";
  path="api/file-import/Client-Profile";
  public importedfile='Choose Client Profile PDF';
  serviceBusinessId:number=0;
  officerId:number=0;
  fileType:string='';
  fileList: DropDownModel[] = [
    {value: 'Passport', text: 'Passport'},
    {value: 'NRIC', text: 'NRIC'},
    {value: 'FIN', text: 'FIN'},
    {value: 'Other', text: 'Other'}
  ];

  constructor(public dialog: MatDialog,public dialogRef: MatDialogRef<ImportDocumentsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataModel,private resourceClient: ResourceClientService,
    private snackBar: MatSnackBar,private http: HttpClient) { 
      this.serviceBusinessId = data.data.serviceBusinessId;
      this.officerId = data.data.officerId;
      if (data.data==='Officer-Profile')
      {
        this.path="api/file-import/Officer-Profile";
        this.importedfile='Choose Officer Profile PDF';
      } 
      else if (data.data==='Registration')
      {
        this.path="api/file-import/Registration";
        this.importedfile='Choose Registration document PDF';
      } 
      else if (data.data==='Entity-Shareholder')
      {
        this.path="api/file-import/entity-shareholder";
        this.importedfile='Choose Registration document PDF';
      } 
    }

  ngOnInit(): void {
    this.path = this.resourceClient.REST_API_SERVER + this.path; 
  }

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'}); 
  }
  OnOK(){
    this.dialogRef.close({event:'Upload',data:this.importedfile});
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
 
    let fileToUpload = <File>files[0];
    if (fileToUpload.type==='application/pdf')
    {
      const formData = new FormData();
      formData.append('file', fileToUpload, fileToUpload.name);
      
      this.http.post(this.path, formData, {reportProgress: true, observe: 'events'})
        .subscribe(event => {
          if (event.type === HttpEventType.UploadProgress)
            this.progress = Math.round(100 * event.loaded / event.total);
          else if (event.type === HttpEventType.Response) {
            this.message = 'Upload success.';
            this.onUploadFinished.emit(event.body);
            this.OnShowMessage ("File Imported.")
            this.importedfile = event.body.toString();

  
          }
        });
    }
    else
    {
      this.OnShowMessage ("Only PDF files are allowed.")
    }
  }
  
}

