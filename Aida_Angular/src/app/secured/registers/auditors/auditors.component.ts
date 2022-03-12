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

import { AuditorsDetailComponent } from '../auditors-detail/auditors-detail.component';

const moment = _moment;

export interface DataModel {
  
  entryDate: string;
  name: string;
  address: string;
  registrationNo:String,
  startDate: string;
  cessationDate: string;
  remarks1: string;
  remarks2: string;
  remarks3: string;
  businessProfileId:number
}


@Component({
  selector: 'app-auditors',
  templateUrl: './auditors.component.html',
  styles: []
})
export class AuditorsComponent implements OnInit {

  pageTitle = "Register of Auditors";
  addToolTip = "Add  new Auditors details";
  filterCaption = "Search in Auditors details";
  customIcon: any ={ iconName: 'file-download', prefix: 'fas' };;
  customToolTip: string='Download register';

  clientProfileId='0';

  companyNameList: DropDownModel[] = [];

  rootPath="api/register/auditors-registers";

  displayedColumns: string[] = ['entryDate','name','registrationNo','startDate','cessationDate','remarks1','toolbox'];
  
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

    this.route.params.subscribe(params => {
      this.clientProfileId = params['clientProfileId'] ;
      this.OnLoad();
    });
    
  }
  OnLoad(){
    this.resourceClient.getDataInGet(this.rootPath+'/'+this.clientProfileId).subscribe((data: DataModel[])=>{
      this.dataSource = new MatTableDataSource<DataModel>(data);
      this.dataSource.paginator = this.paginator;
    })  
  }
  OnAdd()
  {
    this.OnEdit('Add',{id: 0,entryDate: null,name: '',address: '',registrationNo:'',startDate: null,cessationDate: null,
    remarks1: '',remarks2: '',remarks3: '',businessProfileId:this.clientProfileId});
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(AuditorsDetailComponent, {
      width: '1200px',
      data:{action:option,data:paramData,officers:[]},disableClose: true
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
        
        aDate   = moment(result.data.startDate, 'YYYY-MM-DD', true);

        if (aDate.isValid())
          result.data.startDate = result.data.startDate.format('YYYY-MM-DD') ;
        else
          result.data.startDate = null; 

          aDate   = moment(result.data.cessationDate, 'YYYY-MM-DD', true);

          if (aDate.isValid())
            result.data.cessationDate = result.data.cessationDate.format('YYYY-MM-DD') ;
          else
            result.data.cessationDate = null; 

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
    this.resourceClient.getDataInGet('api/register/download-auditors-registers'+'/'+this.clientProfileId).subscribe((data: any)=>{
      
      window.open(this.resourceClient.REST_API_SERVER + 'api/register-download/'+data,'_blank') ;

    });
  }
} 