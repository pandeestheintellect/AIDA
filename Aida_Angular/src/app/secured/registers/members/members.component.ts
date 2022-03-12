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

import { MembersDetailComponent } from '../members-detail/members-detail.component';

const moment = _moment;

export interface DataModel {
  id: number;
  entryDate: string;
  allotmentDate: string;
  allotmentNo: string;
  certificateNo:String,
  price:number,
  srFrom:string,
  srTo:string,
  acquisitions:number,
  disposals:number,
  balance: number;
  classOfShare:string,
  businessProfileId:number,  
  officerId:number,
  officerName: string;
}


@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styles: []
})
export class MembersComponent implements OnInit {

  pageTitle = "Register of Members";
  addToolTip = "Add  new Members share details";
  filterCaption = "Search in Members share details";
  customIcon: any ={ iconName: 'file-download', prefix: 'fas' };;
  customToolTip: string='Download register';

  clientProfileId='0';
  officerId='0';

  companyNameList: DropDownModel[] = [];
  officerList: DropDownModel[] = [];
  identyTypeList: DropDownModel[] = [];

  rootPath="api/register/members-registers";

  displayedColumns: string[] = ['officerName','allotmentDate','allotmentNo','certificateNo','acquisitions','balance','toolbox'];
  
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
    this.OnEdit('Add',{id: 0,entryDate: null, allotmentDate: null,allotmentNo: '',srFrom: 0, srTo:0,
      price:0, certificateNo:'',businessProfileId:this.clientProfileId,  acquisitions:0,
      disposals:0,balance: 0, classOfShare:'Ordinary Share',
      officerId:this.officerId,officerName: ''});
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(MembersDetailComponent, {
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
        
        var aDate   = moment(result.data.entryDate, 'YYYY-MM-DD', true);

        if (aDate.isValid())
          result.data.entryDate = result.data.entryDate.format('YYYY-MM-DD') ;
        else
          result.data.entryDate = null; 
        
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
    this.resourceClient.getDataInGet('api/register/download-members-registers'+'/'+this.clientProfileId+'/'+this.officerId).subscribe((data: any)=>{
      
      window.open(this.resourceClient.REST_API_SERVER + 'api/register-download/'+data,'_blank') ;

    });
  }
} 