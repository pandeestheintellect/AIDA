import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';

import {MatPaginator} from '@angular/material/paginator';
import { MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';
import { MatTableExporterDirective } from 'mat-table-exporter';
import { ResponseModel,DropDownModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';

import * as _moment from 'moment';
import { ApplicationsAllotmentsDetailComponent } from '../applications-allotments-detail/applications-allotments-detail.component';

const moment = _moment;

export interface DataModel {
  id: number;
  applicationDate: string;
  allotmentDate: string;
  allotmentNo: string;
  applied: number;
  allotted:number,
  price:number,
  amountCalled:number,
  amountPaid:number,
  consideration:string,
  certificateNo:String,
  folioNo:string
  businessProfileId:number,  
  officerId:number,
  officerName: string;
}

@Component({
  selector: 'app-applications-allotments',
  templateUrl: './applications-allotments.component.html',
  styles: []
})
export class ApplicationsAllotmentsComponent implements OnInit {

  pageTitle = "Register of Applications and Allotments";
  addToolTip = "Add  new application and allotment";
  filterCaption = "Search in Applications and Allotments";
  customIcon: any ={ iconName: 'file-download', prefix: 'fas' };;
  customToolTip: string='Download register';

  clientProfileId='0';
  officerId='0';

  companyNameList: DropDownModel[] = [];
  officerList: DropDownModel[] = [];
  identyTypeList: DropDownModel[] = [];

  rootPath="api/register/applications-allotments";

  displayedColumns: string[] = ['officerName','applicationDate','allotmentDate','allotmentNo','applied','allotted','toolbox'];
  
  dataSource = new MatTableDataSource<DataModel>();

  //@ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('exporter',{ static: true}) exporter: MatTableExporterDirective;
  
  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/masters/dropdown/business-profile').subscribe((data: DropDownModel[])=>{
      this.companyNameList = data;
    });
    this.resourceClient.getDataInGet('api/masters/dropdown/identity-type').subscribe((data: DropDownModel[])=>{
      this.identyTypeList = data;
    });
    this.route.params.subscribe(params => {
      this.clientProfileId = params['clientProfileId'] ;
      this.onLoadOfficer();
      this.OnLoad();
    });

    
  }
  onLoadOfficer()
  {
    this.resourceClient.getDataInGet('api/business-officers-dropdown/'+this.clientProfileId).subscribe((data: DropDownModel[])=>{
      this.officerList = data;
    });



  }
  OnLoad(){
    this.resourceClient.getDataInGet(this.rootPath+'/'+this.clientProfileId+'/'+this.officerId).subscribe((data: DataModel[])=>{
      this.dataSource = new MatTableDataSource<DataModel>(data);
      this.dataSource.paginator = this.paginator;
    })  
  }
  OnAdd()
  {
    this.OnEdit('Add',{id: 0,applicationDate: null, allotmentDate: null,allotmentNo: '',applied: 0, alloted:0,
      price:0,amountCalled:0,amountPaid:0,consideration:'', certificateNo:'',folioNo:'',businessProfileId:this.clientProfileId,  
      officerId:this.officerId,officerName: ''});
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(ApplicationsAllotmentsDetailComponent, {
      width: '1200px',
      data:{action:option,data:paramData,officers:this.officerList,identityTypes:this.identyTypeList},disableClose: true
    });

    
    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
        if(result.event==='Delete')
        {
          this.resourceClient.getDataInDelete(this.rootPath,result.data.id).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          });
          return; 
        }
        
        var aDate   = moment(result.data.applicationDate, 'YYYY-MM-DD', true);

        if (aDate.isValid())
          result.data.applicationDate = result.data.applicationDate.format('YYYY-MM-DD') ;
        else
          result.data.applicationDate = null; 
        
        aDate   = moment(result.data.allotmentDate, 'YYYY-MM-DD', true);

        if (aDate.isValid())
          result.data.allotmentDate = result.data.allotmentDate.format('YYYY-MM-DD') ;
        else
          result.data.allotmentDate = null; 

        if(result.event==='Add')
        {

          this.resourceClient.getDataInPost(this.rootPath,result.data).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          }) 
        }
        else 
        {

          this.resourceClient.getDataInPut(this.rootPath,result.data).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          }) 
        }
      }
      
    });

  }

  OnGet()
  {
    this.OnLoad();
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
  OnCustom()
  {
    this.resourceClient.getDataInGet('api/register/download-applications-allotments'+'/'+this.clientProfileId+'/'+this.officerId).subscribe((data: any)=>{
      
      window.open(this.resourceClient.REST_API_SERVER + 'api/register-download/'+data,'_blank') ;

    });
  }
} 