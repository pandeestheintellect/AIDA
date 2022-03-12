
import { Component, OnInit, ViewChild,AfterViewInit } from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table'; 
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';
import { MatTableExporterDirective } from 'mat-table-exporter';
import { MatSort} from '@angular/material/sort';
import { ResponseModel} from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { SendFormModel, ServiceRegistraionDocument} from '../../../shared/models/service-data';
import { UploadComponent } from '../upload/upload.component';


import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';

import * as _moment from 'moment';

import { SendFormComponent } from 'src/app/shared/components/send-form/send-form.component';

const moment = _moment;

@Component({
  selector: 'app-clients',
  templateUrl: './employment-agency.component.html',
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class EmploymentAgencyComponent
implements OnInit , AfterViewInit{

  codec :HttpUrlEncodingCodec = new HttpUrlEncodingCodec;

  pageTitle = 'Employment Agency';
  addToolTip = "Add new Jobseeker"; 

  filterCaption = "Search in Jobseeker";

  documentList:SendFormModel={businessProfileId: 0,name: '',email:'',mobile:'',message: '',documentType:'Employment',
                  documentNames:[{stepId:1, documentName:'PDPA Consent-Candidate-EE',filePath:'ABS PDPA Consent-Candidate-EE.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
                                 {stepId:2, documentName:'PDPA Consent-Customer V-20181220',filePath:'ABS PDPA Consent-Customer V-20181220.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
                                 {stepId:2, documentName:'PDPA Withdrawal-All',filePath:'ABS PDPA Withdrawal-All.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
                                 {stepId:2, documentName:'SPL MOM Authorise Form-FW V-PE',filePath:'ACHI Biz SPL MOM Authorise Form-FW V-PE.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
                                 {stepId:2, documentName:'EA-Agreement-Only For Doc LV-03',filePath:'Z-3-ACHI Biz EA-Agreement-Only For Doc LV-03.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
                                 {stepId:2, documentName:'EA-Employee-Authorisation Letter',filePath:'Z-4-ACHI Biz EA-Employee-Authorisation Letter V-03.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
                                 {stepId:2, documentName:'EA-Employee-Particulars-Declaration-LOC',filePath:'Z-5-ACHI Biz EA-Employee-Particulars-Declaration-LOC V-03.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
                                 {stepId:2, documentName:'EA-Employee-Particulars-Declaration-WP-SP-EP',filePath:'Z-5-ACHI Biz EA-Employee-Particulars-Declaration-WP-SP-EP V-03.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
                                 {stepId:2, documentName:'EA-Pass holder-Authorisation For LTVP',filePath:'Z-6-ACHI Biz EA-Pass holder-Authorisation For LTVP.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''}]
                                }

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

    this.dataSource.data.length=0;    
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    var firstDay = new Date(y, m, 1);
    var lastDay = new Date(y, m + 1, 0);

    this.startDate = moment(firstDay,'DD/MM/YYYY') ;
    this.endDate = moment(lastDay,'DD/MM/YYYY');
  
    
    this. OnGet();

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
    
    this.resourceClient.getDataInGet('api/service-execution-send-document/Employment/'+ this.startDate.format('YYYY-MM-DD') +'/'+this.endDate.format('YYYY-MM-DD')).subscribe((data: ServiceRegistraionDocument[])=>{
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

  OnAdd()
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