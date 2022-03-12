


import { Component, OnInit, ViewChild,Inject } from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';

import { ResponseModel,DropDownModel,DialogDataModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import {SelectionModel} from '@angular/cdk/collections';
import { ServicesSOPModel,ServiceRegistraionDocument, SendFormModel } from '../../../shared/models/service-data';

import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'app-send-form',
  template: `
    <h2 mat-dialog-title>{{pageTitle}}</h2>

<div class="app-dialog" mat-dialog-content>

    <div fxLayout="row wrap" class="edit-section">
        <div fxFlex="auto" fxFlex.gt-sm="55%" style="padding: 20px;">
            
            <div fxLayout="row wrap" class="edit-section">
                <mat-form-field fxFlex="auto" fxFlex.gt-sm="100%">
                    <mat-label>Name</mat-label><input matInput [(ngModel)]="localData.name">
                </mat-form-field>
                <mat-form-field fxFlex="auto" fxFlex.gt-sm="70%">
                    <mat-label>Email</mat-label><input matInput [(ngModel)]="localData.email">
                </mat-form-field>
                <mat-form-field fxFlex="auto" fxFlex.gt-sm="30%">
                    <mat-label>Mobile</mat-label><input matInput [(ngModel)]="localData.mobile">
                </mat-form-field>
                <angular-editor [placeholder]="'Enter text here...'" [config]="editorConfig" [(ngModel)]="messageData"></angular-editor>
                
            </div>
            
        </div>
        <div fxFlex="auto" fxFlex.gt-sm="45%" style="padding: 20px;">
            <table class="table" mat-table [dataSource]="dataSource" *ngIf="dataSource.data.length>0" >
                <ng-container matColumnDef="select">
                    <th mat-header-cell *matHeaderCellDef>
                      <mat-checkbox color="primary" (change)="$event ? masterToggle() : null"
                                    [checked]="selection.hasValue() && isAllSelected()"
                                    [indeterminate]="selection.hasValue() && !isAllSelected()">
                      </mat-checkbox>
                    </th>
                    <td mat-cell *matCellDef="let row">
                      <mat-checkbox color="primary"  (click)="$event.stopPropagation()"
                                    (change)="$event ? selection.toggle(row) : null"
                                    [checked]="selection.isSelected(row)">
                      </mat-checkbox>
                    </td>
                  </ng-container>
        
        

              <ng-container matColumnDef="documentName">
                  <th mat-header-cell *matHeaderCellDef> Choose the documents </th>
                  <td mat-cell *matCellDef="let element"> {{element.documentName}} </td>
              </ng-container>
        
                <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
              </table>
        
           
        </div>


    </div>
  
</div>
<div mat-dialog-actions style="justify-content: flex-end;">
    <button mat-button (click)="OnOK()"    mat-flat-button color="primary">Ok</button>
    <button mat-button (click)="OnClose()" mat-flat-button color="warn">Cancel</button>
</div>    
  `,
  styles: []
})
export class SendFormComponent implements OnInit {

  editorConfig: AngularEditorConfig = {
    editable: true,
      spellcheck: true,
      height: '60%',
      minHeight: '0',
      maxHeight: '60%',
      width: '100%',
      minWidth: '0',
      translate: 'yes',
      enableToolbar: false,
      showToolbar: false,
      placeholder: 'Enter text here...',
      defaultParagraphSeparator: '',
      defaultFontName: 'Calibri',
      defaultFontSize: '',
      fonts: [
        {class: 'arial', name: 'Arial'},
        {class: 'times-new-roman', name: 'Times New Roman'},
        {class: 'calibri', name: 'Calibri'},
        {class: 'comic-sans-ms', name: 'Comic Sans MS'}
      ],
      customClasses: [
      {
        name: 'quote',
        class: 'quote',
      },
      {
        name: 'redText',
        class: 'redText'
      },
      {
        name: 'titleText',
        class: 'titleText',
        tag: 'h1',
      },
    ],
    uploadUrl: '',
    uploadWithCredentials: false,
    sanitize: true,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      ['bold', 'italic'],
      ['fontSize']
    ]
};
  messageData: any = `
  <p><font face="Calibri">Warm Greetings From ACHI BIZ!</font></p><p><font face="Calibri"><br></font></p><p><font face="Calibri"><br></font></p><p><font face="Calibri"><br></font></p><p><font face="Calibri">Please do not hesitate to call us for any further query or information which will be served with great pleasure.</font></p><p><font face="Calibri"><br></font></p><p><font face="Calibri">Thank You &amp; Best Regards!</font></p><p><font face="Calibri"><br></font></p><p><font face="Calibri">Have A Great Day!</font></p><p><font face="Calibri"><br></font></p><p><font face="Calibri">[Mr] ACHI KUMAR, B.Com, ACS, ACG.</font></p><p><font face="Calibri">Director</font></p><p><font face="Calibri">ACHI BIZ SERVICES PTE. LTD.</font></p><p><font face="Calibri">20 MAXWELL ROAD, #07-18A MAXWELL HOUSE, SINGAPORE 069113.</font></p><p><font face="Calibri">ACRA UEN: 201415822C &amp; ACRA RFA: FA20143418</font></p><p><font face="Calibri">MOM EA Lic. No.: 18C9185 &amp; MOM EAP Regn. No.: R1874406</font></p><p><font face="Calibri">MOM EAP Name: MARUTHAN CHETTY ACHIKUMAR</font></p><p><font face="Calibri">T: (+65) 69048665 | F: (+65) 69048664 | M: (+65) 91097753 | W: (+65) 91097753</font></p><p><font face="Calibri">E: biz@achibiz.com | S: achibizservicespl | W: www.achibiz.com</font></p><p><font face="Calibri">~~~~~~~~~~~~~~~~~~~~~~</font></p><p><font face="Calibri">A Corporate Hallmark of International Biz Services for:</font></p><p><font face="Calibri">Incorporation, Corporate Secretarial, Compliance, Biz Consultancy, Bookkeeping,&#160;</font></p><p><font face="Calibri">Annual Reports, Taxation, HR Functions, Employment Agency, Immigration, Etc.&#160;</font></p><p><font face="Calibri">~~~~~~~~~~~~~~~~~~~~~~</font></p> `;

  displayedColumns: string[] = ['select','documentName'];
  documents:ServicesSOPModel[] =[];
  documentType:string='';
  pageTitle:string='';
  dataSource = new MatTableDataSource<ServicesSOPModel>(this.documents);

  selection = new SelectionModel<ServicesSOPModel>(true, []);
  localData:ServiceRegistraionDocument={id:0, name:'',email:'',mobile:'',documentNames:'',filePaths:'',documentType:'',
     message:'Welcome Message',status:'New'};
  
  constructor(public dialogRef: MatDialogRef<SendFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SendFormModel,private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private snackBar: MatSnackBar)
    { 
      this.localData.id = data.businessProfileId;
      this.localData.name = data.name;
      this.localData.email=data.email;
      this.localData.mobile = data.mobile;
      this.documents = data.documentNames;
      this.dataSource = new MatTableDataSource<ServicesSOPModel>(this.documents);
      this.localData.documentType=data.documentType;
      if (this.localData.documentType==='ec-clients')
        this.pageTitle = 'Send EC Clients Checklist';
      else if (this.localData.documentType==='nc-clients')
        this.pageTitle = 'Send NC Clients Checklist';
      else
        this.pageTitle = 'Send ' + this.localData.documentType + ' Documents';

    } 

  ngOnInit(): void {
    
  }

  OnOK(){

    var docs:string[]=[];
    var paths:string[]=[];
    if (this.localData.email.length===0)
    {
      this.OnShowMessage('Please enter a valid email');
      return;
    }
    this.selection.selected.forEach( 
      row => {
        docs.push(row.documentName);
        paths.push(row.filePath);
        });
    if (docs.length>0)
    {
      this.localData.documentNames=docs.join(', ');
      this.localData.filePaths=paths.join(',');
      this.localData.message=this.messageData;
      
      this.OnShowMessage('Please wait sending mail...')
      this.resourceClient.getDataInPost('api/service-execution-send-document',this.localData).subscribe((data: ResponseModel)=>{
        this.OnShowMessage(data.message);
        if (data.isSuccess===true)
          this.dialogRef.close({event:'Update',data:'Saved'});
      })  
    }
    else
      this.OnShowMessage('Please select a document')

    
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }


  
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.dataSource.data.forEach(row => this.selection.select(row));
  }

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 4000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }
}
