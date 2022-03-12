import { Component, OnInit, ViewChild } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatSnackBar} from '@angular/material/snack-bar';

import { ResourceClientService } from '../../../service/resourceclient.service';
import { ResponseModel } from '../../../shared/models/common-data';
import { CompanyModel } from '../../../shared/models/company-data';
import { CompanyDetailComponent } from '../company-detail/company-detail.component';

import * as moment from 'moment'; 

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html'
})

export class CompanyComponent implements OnInit {

  pageTitle = "Company Listing";
  addToolTip = "Add  new Company"; 
  rootPath="api/companies"; 

  displayedColumns: string[] = ['companyUEN','companyName','mobile','email','industryType','status','toolbox'];

  dataSource = new MatTableDataSource<CompanyModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    
    this.OnLoad();
  }

  OnLoad()
  {
    
    this.resourceClient.getDataInGet(this.rootPath).subscribe((data: CompanyModel[])=>{
      this.dataSource = new MatTableDataSource<CompanyModel>(data);
      this.dataSource.paginator = this.paginator;
    })  
    
  }
  OnAdd()
  {
    this.OnEdit('Add', {companyId: 0,companyName: '',companyUEN: '',incorpDate: null,address1:'' ,address2: '',city: '',country: '',
    pincode:'' ,mobile: '',email: '',fax: '',gstRegNo: '',industryType: '',status: '',statusDate: null});
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(CompanyDetailComponent, {
      width: '1200px',
      data:{action:option,data:paramData},disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
        if(result.event==='Delete')
        {
          this.resourceClient.getDataInDelete(this.rootPath,result.data.companyId).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          });
          return; 
        }

        var aDate   = moment(result.data.incorpDate, 'YYYY-MM-DD', true);

        if (aDate.isValid())
          result.data.incorpDate = result.data.incorpDate.format('YYYY-MM-DD') ;
        else
          result.data.incorpDate = null; 

        aDate   = moment(result.data.statusDate, 'YYYY-MM-DD', true);

        if (aDate.isValid())
          result.data.statusDate = result.data.statusDate.format('YYYY-MM-DD') ;
        else
          result.data.statusDate = null; 
          
        if(result.event==='Add')
        { 
          this.resourceClient.getDataInPost(this.rootPath,result.data).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          }) 
        }
        else if(result.event==='Update')
        {
          this.resourceClient.getDataInPut(this.rootPath,result.data).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          }) 
        }
        
        
      }
      
    });

  }
  OnExport(exportType){

  }

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }

}
