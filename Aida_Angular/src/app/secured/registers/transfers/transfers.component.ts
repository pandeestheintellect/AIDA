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

import { TransfersDetailComponent } from '../transfers-detail/transfers-detail.component';

const moment = _moment;

export interface DataModel {
  id: number;
  businessProfileId:number,
  transferNo: string;
  transferDate: string;

  transferorId: number;
  transferorFolioNo: string;
  transferorNoShares: string;
  transferorCertificateNo: string;
  balanceNoShares: string;
  balanceCertificateNo: string;
  transferorName: string;

  transfereeId: string;
  transfereeFolioNo: string;
  transfereeNoShares: string;
  transfereeCertificateNo: string;
  transfereeName: string;
}


@Component({
  selector: 'app-transfers',
  templateUrl: './transfers.component.html',
  styles: []
})
export class TransfersComponent implements OnInit {

  pageTitle = "Register of Transfers";
  addToolTip = "Add  new Transfers details";
  filterCaption = "Search in Transfers details";
  customIcon: any ={ iconName: 'file-download', prefix: 'fas' };;
  customToolTip: string='Download register';

  clientProfileId='0';
  companyNameList: DropDownModel[] = [];
  transferorList: DropDownModel[] = [];
  transferorId='0';

  transfereeList: DropDownModel[] = [];
  transfereeId='0';
  
  rootPath="api/register/transfers-registers";

  displayedColumns: string[] = ['transferNo','transferDate','transferorName','transferorFolioNo','balanceNoShares','transfereeName','transfereeFolioNo','transfereeNoShares','toolbox'];
  
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
      this.onLoadOfficer();
      this.OnLoad();
    });

    
  }
  onLoadOfficer()
  {
    this.resourceClient.getDataInGet('api/business-officers-dropdown/'+this.clientProfileId).subscribe((data: DropDownModel[])=>{
      this.transferorList = data;
      this.transfereeList = data;
    });

  }
  OnLoad(){
    this.resourceClient.getDataInGet(this.rootPath+'/'+this.clientProfileId+'/'+this.transferorId+'/'+this.transfereeId).subscribe((data: DataModel[])=>{
      this.dataSource = new MatTableDataSource<DataModel>(data);
      this.dataSource.paginator = this.paginator;
    })  
  }
  OnAdd()
  {
    this.OnEdit('Add',{id: 0,businessProfileId:this.clientProfileId, transferNo: '',transferDate: null,
    transferorId: 0,transferorFolioNo: '',transferorNoShares: '',transferorCertificateNo: '',balanceNoShares: '',balanceCertificateNo: '',transferorName: '',
    transfereeId: '',transfereeFolioNo: '',transfereeNoShares: '',transfereeCertificateNo: '',transfereeName: '' });
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(TransfersDetailComponent, {
      width: '1200px',
      data:{action:option,data:paramData,officers:this.transfereeList},disableClose: true
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
        
        var aDate   = moment(result.data.transferDate, 'YYYY-MM-DD', true);

        if (aDate.isValid())
          result.data.transferDate = result.data.transferDate.format('YYYY-MM-DD') ;
        else
          result.data.transferDate = null; 

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
    this.resourceClient.getDataInGet('api/register/download-transfers-registers'+'/'+this.clientProfileId+'/'+this.transferorId+'/'+this.transfereeId).subscribe((data: any)=>{
      
      window.open(this.resourceClient.REST_API_SERVER + 'api/register-download/'+data,'_blank') ;

    });
  }
} 