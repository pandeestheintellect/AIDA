import { Component, OnInit, ViewChild,AfterViewInit } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import { MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatSnackBar} from '@angular/material/snack-bar';
import { MatTableExporterDirective } from 'mat-table-exporter';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { DropDownModel } from '../../../shared/models/common-data';
import { DocumentModel } from '../../../shared/models/document-data';
import { Router} from '@angular/router';
import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';
import * as _moment from 'moment';

const moment = _moment;

@Component({
  selector: 'app-services-summary',
  template: `
    
    <div class="mat-elevation-z4 app-page">

    <app-page-toolbar [title]="pageTitle" [addNew]="false" (OnFilter)="OnFilter($event)" 
      [filterCaption]="filterCaption" [hasFilter]="true" (OnExport)="OnExport($event)"
      ></app-page-toolbar>

      <div  fxLayout="row wrap" fxLayoutWrap class="control-bar">
           
           <mat-form-field fxFlex="auto" fxFlex.gt-sm="20%">
               <mat-label>Choose Client Profile</mat-label>
               <mat-select [(ngModel)]="businessProfileId" name="businessProfileName">
                   <mat-option *ngFor="let type of companyNameList" [value]="type.value">{{type.text}}</mat-option>
               </mat-select>
             </mat-form-field>
             <mat-form-field fxFlex="auto" fxFlex.gt-sm="20%">
              <mat-label>UEN</mat-label><input matInput [(ngModel)]="uen">
             </mat-form-field>
             <mat-form-field fxFlex="auto" fxFlex.gt-sm="20%">
                 <mat-label>Incorp Date</mat-label>
                     <input matInput [matDatepicker]="incorpDatepicker" [(ngModel)]="incorpDate">
                     <mat-datepicker-toggle matSuffix [for]="incorpDatepicker"></mat-datepicker-toggle>
                     <mat-datepicker #incorpDatepicker></mat-datepicker>
             </mat-form-field>
             <mat-form-field fxFlex="auto" fxFlex.gt-sm="20%">
              <mat-label>Officer Name (starting)</mat-label><input matInput [(ngModel)]="officerName">
             </mat-form-field>
             <mat-form-field fxFlex="auto" fxFlex.gt-sm="20%">
               <mat-label>Choose Service</mat-label>
               <mat-select [(ngModel)]="serviceCode" name="servicesList">
                   <mat-option *ngFor="let type of servicesList" [value]="type.value">{{type.text}}</mat-option>
               </mat-select>
             </mat-form-field>
             <mat-form-field fxFlex="auto" fxFlex.gt-sm="20%">
               <mat-label>Choose Service Status</mat-label>
               <mat-select [(ngModel)]="status" name="statusList">
                   <mat-option *ngFor="let type of statusList" [value]="type.value">{{type.text}}</mat-option>
               </mat-select>
             </mat-form-field>
             <mat-form-field fxFlex="auto" fxFlex.gt-sm="20%">
              <mat-label>Document Name (starting)</mat-label><input matInput [(ngModel)]="documentName">
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
            
           <div style="padding: 10px;height: 40px;" fxFlex="auto" fxFlex.gt-sm="12%">
               <button mat-button mat-flat-button  (click)="OnGet()" color="primary">Get</button>
           </div>
     </div>

    <table class="table" mat-table [dataSource]="dataSource" matSort matTableExporter #exporter="matTableExporter">
        <ng-container matColumnDef="businessProfileName">
          <th mat-header-cell *matHeaderCellDef  mat-sort-header> Client Name </th> 
          <td mat-cell *matCellDef="let element"> {{element.businessProfileName}} </td>
        </ng-container>
        
        <ng-container matColumnDef="uen">
          <th mat-header-cell *matHeaderCellDef mat-sort-header> UEN </th>
          <td mat-cell *matCellDef="let element"> {{element.uen}} </td>
        </ng-container>
        <ng-container matColumnDef="incorpDate">
          <th mat-header-cell *matHeaderCellDef  mat-sort-header> Incorp Date </th>
          <td mat-cell *matCellDef="let element"> {{element.incorpDate}} </td>
        </ng-container>
        
        <ng-container matColumnDef="serviceName">
          <th mat-header-cell *matHeaderCellDef  mat-sort-header> Service </th> 
          <td mat-cell *matCellDef="let element"> {{element.serviceName}} </td>
        </ng-container>
        <ng-container matColumnDef="createdDate">
          <th mat-header-cell *matHeaderCellDef  mat-sort-header> Created Date </th>
          <td mat-cell *matCellDef="let element"> {{element.createdDate}} </td>
        </ng-container>
        <ng-container matColumnDef="status">
          <th mat-header-cell *matHeaderCellDef  mat-sort-header> Status </th>
          <td mat-cell *matCellDef="let element"> {{element.status}} </td>
        </ng-container>
        
        <ng-container matColumnDef="toolbox">
            <th mat-header-cell *matHeaderCellDef width ="40"> </th>
            <td mat-cell *matCellDef="let element"> 
                <mat-button-toggle-group #group="matButtonToggleGroup">
                    <mat-button-toggle value="center" aria-label="view details of {{element.businessProfileName}}" (click)="OnView(element)">
                      <fa-icon [icon]="['fas', 'arrow-right']"></fa-icon>
                    </mat-button-toggle>

                  </mat-button-toggle-group>
            </td>
          </ng-container>
    
          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
        </table>
      
        <mat-paginator [pageSize]="8" [pageSizeOptions]="[8, 20,40,80]" style="padding-bottom: 30px;">
        </mat-paginator>
        
    </div>  

  `,
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class ServicesSummaryComponent implements OnInit {

  pageTitle = "Service Summary";
  customIcon: any ={ iconName: 'filter', prefix: 'fas' };;
  customToolTip: string='Add filter on the summary';
  filterCaption = "Search in results";
  rootPath="api/documents";

  serviceCode='';
  status='';
  businessProfileId=0;  
  uen='';

  officerName='';
  documentName='';
  incorpDate=null;
  startDate = moment(new Date(),'DD/MM/YYYY').day(-30);
  endDate = moment(new Date(),'DD/MM/YYYY').add(1,'days');
  
  companyNameList: DropDownModel[] = [];
  statusList: DropDownModel[] = [];
  servicesList: DropDownModel[] = [];

  displayedColumns: string[] = ['businessProfileName','uen','incorpDate','serviceName','createdDate','status', 'toolbox'];
  
  dataSource = new MatTableDataSource<DocumentModel>();

  //@ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('exporter',{ static: true}) exporter: MatTableExporterDirective;

  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,private router: Router, 
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    
    this.resourceClient.getDataInGet('api/masters/dropdown/business-profile').subscribe((data: DropDownModel[])=>{
      this.companyNameList = data;
    });

    this.resourceClient.getDataInGet('api/masters/dropdown/services').subscribe((data: DropDownModel[])=>{
      this.servicesList = data;
    });

    this.resourceClient.getDataInGet('api/masters/dropdown/services-status').subscribe((data: DropDownModel[])=>{
      this.statusList = data;
    });

    //this.OnLoad();
  }

  OnLoad()
  {
    
    this.resourceClient.getDataInGet(this.rootPath).subscribe((data: DocumentModel[])=>{
      this.dataSource.data = data ;
    })  
    
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;

  }

  
  OnGet()
  {

    var hasFiter:boolean=false,
        uenLocal:string=this.uen.trim(),
        officerNameLocal:string=this.officerName.trim(),
        serviceCodeLocal:string=this.serviceCode.trim(),
        statusLocal:string=this.status.trim(),
        documentNameLocal:string=this.documentName.trim(),
        incorpDateLocal=this.GetFormatedDate(this.incorpDate),
        startDateLocal=this.GetFormatedDate(this.startDate),
        endDateLocal=this.GetFormatedDate(this.endDate);
    
    if (this.businessProfileId>0)
      hasFiter=true;

    if (uenLocal==='')
      uenLocal='A';
    else  
      hasFiter=true;
      
    if (incorpDateLocal===null)
      incorpDateLocal='A';
    else  
    {
      incorpDateLocal=this.incorpDate.format('YYYY-MM-DD');
      hasFiter=true;
    }
      
    
    if (officerNameLocal==='')
      officerNameLocal='A';
    else  
      hasFiter=true;
        
    if (serviceCodeLocal==='')
      serviceCodeLocal='A';
    else  
      hasFiter=true;
    
    if (statusLocal==='')
      statusLocal='A';
    else  
      hasFiter=true;
        
    if (documentNameLocal==='')
      documentNameLocal='A';
    else  
      hasFiter=true;
    
    if (startDateLocal===null)
      startDateLocal='A';
    else  
    {
      startDateLocal=this.startDate.format('YYYY-MM-DD');
      hasFiter=true;
    }
    
    if (endDateLocal===null)
      endDateLocal='A';
    else  
    {
      endDateLocal=this.endDate.format('YYYY-MM-DD');
      hasFiter=true;
    }
    
    if (hasFiter)
    {
      var data = {businessProfileId:this.businessProfileId, uen:uenLocal, officerName:officerNameLocal, serviceCode:serviceCodeLocal,
        status:statusLocal,documentName:documentNameLocal, incorpDate:incorpDateLocal, startDate:startDateLocal, endDate:endDateLocal}

        this.resourceClient.getDataInPost('api/service-summary',data).subscribe((data: any[])=>{
          this.dataSource = new MatTableDataSource<any>(data);
          this.dataSource.paginator = this.paginator;
        })  
    }
    /*

    
    if (this.endDate.isBefore(this.startDate) )
    {
      this.OnShowMessage("Start date can not be after End date");
      return;
    }

    if (this.businessProfileId.length>0 || this.status.length>0)
    {
      if (this.businessProfileId=='')
        this.businessProfileId='0';
      if (this.status=='')
        this.status='A';
      this.resourceClient.getDataInGet('api/service-registration/'+this.serviceCode+'/'+this.businessProfileId+'/'+this.status+'/'+ this.startDate.format('YYYY-MM-DD') +'/'+this.endDate.format('YYYY-MM-DD')).subscribe((data: any[])=>{
        this.dataSource = new MatTableDataSource<any>(data);
        this.dataSource.paginator = this.paginator;
      })  
      
    }
    else
      this.OnShowMessage("Please choose any one option to continue"); 
    */
  }
  OnExport(exportType){
    if (exportType===1)
      this.exporter.exportTable('xlsx', {fileName:'AIDAExport', sheet: 'sheet_name', Props: {Author: 'AIDA'}})
  }
  OnFilter(filterText:string){
    this.dataSource.filter = filterText.trim().toLowerCase();
  }
  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }

  OnView(data:any) {
    this.router.navigate(['/secured/services/listing',
        data.serviceCode.toLowerCase(), data.businessProfileId,data.status.toLowerCase(),data.serviceBusinessId]);
  }
  GetFormatedDate(datestring)
  {
    var aDate   = moment(datestring, 'YYYY-MM-DD', true);

    if (aDate.isValid())
      return datestring.format('YYYY-MM-DD') ; 
    else
      return null; 

  }
}
