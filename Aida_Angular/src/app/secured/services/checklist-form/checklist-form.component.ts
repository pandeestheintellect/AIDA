
import { Component, OnInit, ViewChild,AfterViewInit } from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table'; 
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';
import { MatTableExporterDirective } from 'mat-table-exporter';
import { MatSort} from '@angular/material/sort';
import { DropDownModel, ResponseModel} from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { SendFormModel, ServiceRegistraionDocument} from '../../../shared/models/service-data';
import { UploadComponent } from '../upload/upload.component';


import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';

import * as _moment from 'moment';

import { SendFormComponent } from 'src/app/shared/components/send-form/send-form.component';

import { documentsData } from './documents-data';
import { ClientOfficerModel } from 'src/app/shared/models/client-officer-data';

const moment = _moment;


@Component({
  selector: 'app-checklist-form',
  template: `
    <div class="mat-elevation-z4 app-page">

<app-page-toolbar [title]="pageTitle" [addToolTip]="addToolTip" (OnAdd)="OnAdd()" (OnFilter)="OnFilter($event)" 
[filterCaption]="filterCaption" [hasFilter]="true" (OnExport)="OnExport($event)"></app-page-toolbar>

<div  fxLayout="row" fxLayoutWrap class="control-bar" style="padding-bottom: 0px !important;">
       
      <mat-form-field style="width: 250px; font-size: 14px !important;">
                <mat-label>Choose Client Profile</mat-label>
                <mat-select [(ngModel)]="clientProfileId" name="clientProfileName">
                    <mat-option *ngFor="let type of clientProfileList" [value]="type.value">{{type.text}}</mat-option>
                </mat-select>
            </mat-form-field>
      
        <mat-form-field fxFlex="auto" fxFlex.gt-sm="20%">
            <mat-label>Start Date</mat-label>
                <input matInput [matDatepicker]="startDatepicker" [(ngModel)]="startDate">
                <mat-datepicker-toggle matSuffix [for]="startDatepicker"></mat-datepicker-toggle>
                <mat-datepicker #startDatepicker></mat-datepicker>
        </mat-form-field>

        <mat-form-field fxFlex="auto" fxFlex.gt-sm="20%">
            <mat-label>End Date</mat-label>
                <input matInput [matDatepicker]="endDatepicker" [(ngModel)]="endDate">
                <mat-datepicker-toggle matSuffix [for]="endDatepicker"></mat-datepicker-toggle>
                <mat-datepicker #endDatepicker></mat-datepicker>
        </mat-form-field>
       
      <div style="padding: 10px;height: 40px;" fxFlex="auto" fxFlex.gt-sm="15%">
          <button mat-button mat-flat-button  (click)="OnGet()" color="primary">Get</button>
      </div>
</div>

<table class="table" mat-table [dataSource]="dataSource" *ngIf="dataSource.data.length>0" >
<ng-container matColumnDef="name">
<th mat-header-cell *matHeaderCellDef>Name </th>
<td mat-cell *matCellDef="let element"> {{element.name}} </td>
</ng-container>
<!-- Code Column -->
<ng-container matColumnDef="email">
<th mat-header-cell *matHeaderCellDef> Email </th>
<td mat-cell *matCellDef="let element"> {{element.email}} </td>
</ng-container>
<!-- Name Column -->
<ng-container matColumnDef="mobile">
<th mat-header-cell *matHeaderCellDef> Mobile </th>
<td mat-cell *matCellDef="let element"> {{element.mobile}} </td>
</ng-container>
<!-- Remarks Column -->
<ng-container matColumnDef="documentNames">
<th mat-header-cell *matHeaderCellDef> Documents </th>
<td mat-cell *matCellDef="let element"> {{element.documentNames}} </td>
</ng-container>
<ng-container matColumnDef="status">
<th mat-header-cell *matHeaderCellDef> Status </th>
<td mat-cell *matCellDef="let element"> {{element.status}} </td>
</ng-container>

<ng-container matColumnDef="toolbox">
<th mat-header-cell *matHeaderCellDef width ="40"> </th>
<td mat-cell *matCellDef="let element"> 
<mat-button-toggle-group #group="matButtonToggleGroup">

  <mat-button-toggle value="center"  matTooltip="Registraion Completed" 
    matTooltipClass="snackBarBackgroundColor"(click)="OnEdit('Completed',element)">
      <fa-icon [icon]="['fas', 'thumbs-up']"></fa-icon>
  </mat-button-toggle>
  <mat-button-toggle value="right" matTooltip="Registration Enquiry Terminated" 
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
  styles: [],
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class ChecklistFormComponent implements OnInit {

  pageTitle = '';
  addToolTip = "Send form"; 
  documentsData = documentsData;
  filterCaption = "Search in send form list";
  serviceCode='';
  clientProfileId='0';  
  clientProfileList: DropDownModel[] = [];

  documentList:SendFormModel={businessProfileId: 0,name: '',email:'',mobile:'',message: '',documentType:'',documentNames:[]}

  startDate = moment(new Date(),'DD/MM/YYYY').day(-30);
  endDate = moment(new Date(),'DD/MM/YYYY').add(1,'days');
  
  displayedColumns: string[] = ['name','mobile','documentNames','status','toolbox'];

  dataSource = new MatTableDataSource<ServiceRegistraionDocument>();


  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('exporter',{ static: true}) exporter: MatTableExporterDirective;

  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private snackBar: MatSnackBar) {

      this.dataSource.data.length=0;    
      
  }

  ngOnInit(): void {


    this.route.params.subscribe(params => {
      var id = -1;
      this.serviceCode = params['serviceCode']

      if (this.serviceCode==='ec-clients')
        id=0;
      else if (this.serviceCode==='nc-clients')
        id=1;

      if (id>=0)
      {
        this.pageTitle='Send ' + this.documentsData[id].service + ' checklist documents' 
        this.documentList.documentNames = this.documentsData[id].documents;
        this.documentList.documentType = this.serviceCode;
        this.documentList.documentName = this.documentsData[id].service;
      }
      else
      {
        this.pageTitle='Forms not available for this service' ;
      }  

      this.resourceClient.getDataInGet('api/masters/dropdown/business-profile').subscribe((data: DropDownModel[])=>{
        this.clientProfileList = data;
      })
  
      this.dataSource.data.length=0;    
      var date = new Date(), y = date.getFullYear(), m = date.getMonth();
      var firstDay = new Date(y, m, 1);
      var lastDay = new Date(y, m + 1, 0);
  
      this.startDate = moment(firstDay,'DD/MM/YYYY') ;
      this.endDate = moment(lastDay,'DD/MM/YYYY');
    
      
      this. OnGet();
  
    })  
     
    
  }
  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;

  }
 
 
  OnGet()
  {
    if (this.endDate.isBefore(this.startDate) )
    {
      this.OnShowMessage("Start date can not be after End date");
      return;
    }
    
    this.resourceClient.getDataInGet('api/service-execution-send-document/'+this.serviceCode+'/'+ this.startDate.format('YYYY-MM-DD') +'/'+this.endDate.format('YYYY-MM-DD')).subscribe((data: ServiceRegistraionDocument[])=>{
      this.dataSource = new MatTableDataSource<ServiceRegistraionDocument>(data);
      this.dataSource.paginator = this.paginator;
    })  

  }

  
  OnUpload(paramData)
  {
    const dialogRef = this.dialog.open(UploadComponent, {
      width: '600px',
      data:{action:'Signing',data:paramData},
      disableClose: true 
    });
  }

  GetCompanyId() {
    var id = parseInt(this.clientProfileId);
    if (this.serviceCode==='nc-clients')
    {
      this.OnShowForm();
      return 0;
    }

    else if (id>0)
      return id;
    else
    {
      this.OnShowMessage("Please choose a business profile to continue");
      return -1;
    }
      
  }

  OnAdd()
  {
    var id = this.GetCompanyId();
    if(id>0)
    {
      this.resourceClient.getDataInGet('api/business-authorised-representative/'+id).subscribe((data: ClientOfficerModel)=>{
          this.documentList.businessProfileId=id;
          this.documentList.name=data.name;
          this.documentList.email= data.email;
          this.documentList.mobile = data.mobile;        
          this.OnShowForm();
      })
    }
  }
  OnShowForm()
  {

   const dialogRef = this.dialog.open(SendFormComponent, {
    width: '95%',
    data:this.documentList,
    disableClose: true 
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result)
    {
      if(result.event==='Cancel')
        return;
      if(result.event==='Update')
      {
       var steps:string = result.data;
      }

    }
    
  });
  }

  OnEdit(option,paramData:ServiceRegistraionDocument) 
  {
    if (paramData.status==='In-Progress')
    {
      paramData.status = option;
      this.OnShowMessage('Please wait updating status...')
      this.resourceClient.getDataInPut('api/service-execution-send-document',paramData).subscribe((data: ResponseModel)=>{
        this.OnShowMessage(data.message);
      })  
    }
    else
    {
      this.OnShowMessage('This enquiry is already ' + paramData.status);
    }
  } 


  OnExport(exportType){
    if (exportType===1) 
      this.exporter.exportTable('xlsx', {fileName:'AIDAExport', sheet: 'sheet_name', Props: {Author: 'AIDA'}})
  }
  OnFilter(filterText:string){
    this.dataSource.filter = filterText.trim().toLocaleLowerCase();
  }

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }

}