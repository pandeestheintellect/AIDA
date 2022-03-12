import { Component, OnInit, ViewChild,AfterViewInit } from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table'; 
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';
import { MatTableExporterDirective } from 'mat-table-exporter';
import { MatSort} from '@angular/material/sort';
import { ResponseModel,DropDownModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { ServiceRegistrationClientDisplayModel,ServicesDefinitionModel } from '../../../shared/models/service-data';
import { SigningComponent } from '../signing/signing.component';
import { UploadComponent } from '../upload/upload.component';


import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';

import * as _moment from 'moment';

const moment = _moment;

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class ClientsComponent implements OnInit , AfterViewInit{

  codec :HttpUrlEncodingCodec = new HttpUrlEncodingCodec;

  pageTitle = '';
  addToolTip = "Add  new Clients"; 

  filterCaption = "Search in services";

  serviceCode='';
  service:ServicesDefinitionModel;
  status='';
  businessProfileId='';  
  entity='';  
  startDate = moment(new Date(),'DD/MM/YYYY').day(-30);
  endDate = moment(new Date(),'DD/MM/YYYY').add(1,'days');
  
  companyNameList: DropDownModel[] = [];
  statusList: DropDownModel[] = [];

  displayedColumns: string[] = ['businessProfileName','uen','generatedDate','status','toolbox'];
  
  dataSource = new MatTableDataSource<ServiceRegistrationClientDisplayModel>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('exporter',{ static: true}) exporter: MatTableExporterDirective;

  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private snackBar: MatSnackBar) { 
      this.dataSource.data.length=0;    
    }

  ngOnInit(): void {

    this.dataSource.data.length=0;    
    this.route.params.subscribe(params => {
      this.serviceCode = params['serviceCode'];
      this.entity = params['entity'];
      if (this.serviceCode==null || this.serviceCode==undefined)
        this.serviceCode='';
      if (this.entity==null || this.entity==undefined)
        this.entity='A';

      this.resourceClient.getDataInGet('api/service-definitions/'+this.serviceCode).subscribe((data: ServicesDefinitionModel)=>{
        this.service = data;
        this.pageTitle =  this.service.name;
        this.filterCaption = "Search in "+this.service.name;

      });
      
      var status = params['status'];
      var period = params['period'];

      if (status==null || status==undefined || period==null || period==undefined)
      {
        this.status='A';
        this. OnGet();
        this.status='';
      }
      else
      {

        var firstDay = new Date();
        var lastDay = new Date();

        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        if(period==='yearly')
        {
          firstDay = new Date(y, 3, 1);
          lastDay = new Date(y+1, 3, 0);
        }
        else if (!isNaN(period))
        {
          m=parseInt(period)-1;
          firstDay = new Date(y, m, 1);
          lastDay = new Date(y, m + 1, 0);
        }
        else
        {
          firstDay = new Date(y, m, 1);
          lastDay = new Date(y, m + 1, 0);
        }

        this.startDate = moment(firstDay,'DD/MM/YYYY') ;
        this.endDate = moment(lastDay,'DD/MM/YYYY');
      
        if (status=='started')
          this.status='New';
        else if (status=='ongoing')
          this.status='In-Progress';
        else  if (status=='finished')
          this.status='Finished';  
        else
          this.status=status;  

        this. OnGet();
        this.status='';

      }
        
    });

    this.resourceClient.getDataInGet('api/masters/dropdown/business-profile').subscribe((data: DropDownModel[])=>{
      this.companyNameList = data;
    });

    
    this.resourceClient.getDataInGet('api/masters/dropdown/services-status').subscribe((data: DropDownModel[])=>{
      this.statusList = data;
    });


  }
  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;

  }
  
  GetCompanyId():number {
    var id = parseInt(this.businessProfileId);
    if (id>0)
    {
      return id;
    }
    else
    {
      this.OnShowMessage("Please choose a client profile to continue");
      return 0;
    }
      
  }
  OnGet()
  {
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
      this.resourceClient.getDataInGet('api/service-registration/'+this.serviceCode+'/'+this.businessProfileId+'/'+this.status+'/'+ this.startDate.format('YYYY-MM-DD') +'/'+this.endDate.format('YYYY-MM-DD')+'/'+this.entity).subscribe((data: any[])=>{
        this.dataSource = new MatTableDataSource<ServiceRegistrationClientDisplayModel>(data);
        this.dataSource.paginator = this.paginator;
      })  
      
    }
    else
      this.OnShowMessage("Please choose any one option to continue"); 

  }
  OnCreate(){
    var id = this.GetCompanyId();
    if(id>0)
    {
      
      this.resourceClient.getDataInPut('api/service-registration',
        {serviceCode:this.serviceCode,businessProfileId:id,sessionToken:'sss'}).subscribe((data: ResponseModel)=>{
          this.OnShowMessage(data.message);
          this.resourceClient.getDataInGet('api/service-registration/'+this.serviceCode+'/'+id).subscribe((data: any[])=>{
            this.dataSource = new MatTableDataSource<ServiceRegistrationClientDisplayModel>(data);
            this.dataSource.paginator = this.paginator;
          })  
      })
     
    }
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
    this.router.navigate(['secured/services/registration',this.serviceCode]);
    /*
    if (this.service.hasOptionalDocument==='true')
      this.router.navigate(['secured/services/registration-options',this.serviceCode]);
    else 
      this.router.navigate(['secured/services/registration',this.serviceCode]);
      */
  }

  OnEdit(data)
  {
    if (data.status !=='Terminated')
      this.router.navigate(['secured/services/listing/'+ this.serviceCode +'/'+ data.id+'/'+data.status+'/'+data.serviceBusinessId]); 
  }

  Terminate(data)
  {
    if (data.status !=='Terminated')
      this.resourceClient.getDataInDelete('api/service-registration/',data.serviceBusinessId).subscribe((data: ResponseModel)=>{
        this.OnShowMessage(data.message)
        this.OnGet();
      })  
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