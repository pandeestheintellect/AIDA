import { Component, OnInit,Input } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import {ActivatedRoute,Router} from '@angular/router';
import {UploadedDocumentModel} from '../../../shared/models/service-data';
import {ResourceClientService } from '../../../service/resourceclient.service';
import { MatSnackBar} from '@angular/material/snack-bar';
import { ResponseModel,DropDownModel,DialogDataModel } from '../../../shared/models/common-data';


@Component({
  selector: 'app-uploaded-documents',
  template: `


<div class="mat-elevation-z4 app-page">

<app-page-toolbar [title]="pageTitle" [addNew]="false" (OnExport)="OnExport($event)"></app-page-toolbar>

<table class="table" mat-table [dataSource]="dataSource" *ngIf="dataSource.data.length>0">

<!-- Code Column -->
<ng-container matColumnDef="serviceName">
  <th mat-header-cell *matHeaderCellDef> Service Name </th>
  <td mat-cell *matCellDef="let element"> {{element.serviceName}} </td>
</ng-container>
<!-- Name Column -->
<ng-container matColumnDef="officerName">
  <th mat-header-cell *matHeaderCellDef> Officer </th>
  <td mat-cell *matCellDef="let element"> {{element.officerName}} </td>
</ng-container>
  <!-- Remarks Column -->
  <ng-container matColumnDef="documentType">
    <th mat-header-cell *matHeaderCellDef> Document Types </th>
    <td mat-cell *matCellDef="let element"> {{element.documentType}} </td>
  </ng-container>
  <ng-container matColumnDef="actualFileName">
    <th mat-header-cell *matHeaderCellDef>FileName </th>
    <td mat-cell *matCellDef="let element"> {{element.actualFileName}} </td>
  </ng-container>
  <ng-container matColumnDef="created">
    <th mat-header-cell *matHeaderCellDef>Date </th>
    <td mat-cell *matCellDef="let element"> {{element.created}} </td>
</ng-container>
  <ng-container matColumnDef="toolbox">
    <th mat-header-cell *matHeaderCellDef width ="40"> </th>
    <td mat-cell *matCellDef="let element"> 
    <mat-button-toggle-group #group="matButtonToggleGroup">

      <mat-button-toggle value="center"  matTooltip="Download this document" 
        matTooltipClass="snackBarBackgroundColor"(click)="OnEdit('Download',element)">
          <fa-icon [icon]="['fas', 'download']"></fa-icon>
      </mat-button-toggle>
      <mat-button-toggle value="right" matTooltip="Delete this document" 
      matTooltipClass="snackBarBackgroundColor"(click)="OnEdit('Terminated',element)">
          <fa-icon [icon]="['far', 'trash-alt']"></fa-icon>
      </mat-button-toggle>
    </mat-button-toggle-group>
      
    </td>
  </ng-container>

    <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
    <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
  </table>

  <mat-paginator [pageSizeOptions]="[10, 20, 30]" showFirstLastButtons *ngIf="dataSource.data.length>10"></mat-paginator>
  
</div>


  `,
  styles: []
})
export class UploadedDocumentsComponent implements OnInit {

  pageTitle = '';

  displayedColumns: string[] = ['created','serviceName','officerName','documentType','actualFileName','toolbox'];

  dataSource = new MatTableDataSource<UploadedDocumentModel>();

  constructor(private resourceClient: ResourceClientService,private route: ActivatedRoute,private snackBar: MatSnackBar) { }

  ngOnInit(): void {

    this.route.params.subscribe(params => {
      this.pageTitle = 'Uploaded documents for ' + params['businessProfileName'];
      this.resourceClient.getDataInGet('api/service-execution-uploaded-document/'+params['businessProfileId'])
        .subscribe((data: UploadedDocumentModel[])=>{
          this.dataSource = new MatTableDataSource<UploadedDocumentModel>(data);
      });

    });
  }
  OnExport(paramData)
  {

  }
  OnEdit(option,paramData:UploadedDocumentModel) 
  {
    
    if (option==='Download')
    {
      window.open(this.resourceClient.REST_API_SERVER + 'api/file-upload-download/'+paramData.filePath,'_blank') ;
        
    }
    else
    {
      this.OnShowMessage('Please wait removing the file...')
      this.resourceClient.getDataInDelete('api/service-execution-uploaded-document',paramData.documentId).subscribe((data: ResponseModel)=>{
        this.OnShowMessage(data.message);

        this.resourceClient.getDataInGet('api/service-execution-uploaded-document/'+paramData.businessProfileId)
          .subscribe((data: UploadedDocumentModel[])=>{
            this.dataSource = new MatTableDataSource<UploadedDocumentModel>(data);
        });
        
      })  
    }
    
  } 

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }
}
