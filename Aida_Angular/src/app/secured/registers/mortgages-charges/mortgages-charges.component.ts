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


import { MortgagesChargesDetailComponent } from '../mortgages-charges-detail/mortgages-charges-detail.component';

const moment = _moment;

export interface DataModel {
  entryDate: string;
  name: string;
  address: string;
  shortDescription: string;
  amount: number;
  registrarDate: string;
  noOfCertificate: number;
  dischargeDate: string;
  remarks: string;
  businessProfileId:number
}

@Component({
  selector: 'app-mortgages-charges',
  templateUrl: './mortgages-charges.component.html',
  styles: []
})
export class MortgagesChargesComponent implements OnInit {

  pageTitle = "Register of Mortgages and Charges";
  addToolTip = "Add new Mortgages and Charges details";
  filterCaption = "Search in Mortgages and Charges";
  customIcon: any ={ iconName: 'file-download', prefix: 'fas' };;
  customToolTip: string='Download register';

  clientProfileId='0';

  companyNameList: DropDownModel[] = [];

  rootPath="api/register/mortgages-charges";

  displayedColumns: string[] = ['entryDate','name','shortDescription','amount','dischargeDate','remarks','toolbox'];
  
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
    this.OnEdit('Add',{id: 0,entryDate: null,name: '',address: '',shortDescription: '',amount: 0,registrarDate: null,
    noOfCertificate: '',dischargeDate: '',remarks: '',businessProfileId:this.clientProfileId});
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(MortgagesChargesDetailComponent, {
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
        
        aDate   = moment(result.data.registrarDate, 'YYYY-MM-DD', true);

        if (aDate.isValid())
          result.data.registrarDate = result.data.registrarDate.format('YYYY-MM-DD') ;
        else
          result.data.registrarDate = null; 

          aDate   = moment(result.data.dischargeDate, 'YYYY-MM-DD', true);

          if (aDate.isValid())
            result.data.dischargeDate = result.data.dischargeDate.format('YYYY-MM-DD') ;
          else
            result.data.dischargeDate = null; 

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
    this.resourceClient.getDataInGet('api/register/download-mortgages-charges'+'/'+this.clientProfileId).subscribe((data: any)=>{
      
      window.open(this.resourceClient.REST_API_SERVER + 'api/register-download/'+data,'_blank') ;

    });
  }
} 