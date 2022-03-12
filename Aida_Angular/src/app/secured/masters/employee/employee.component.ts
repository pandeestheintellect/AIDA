import { Component, OnInit, ViewChild } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatSnackBar} from '@angular/material/snack-bar';

import { ResourceClientService } from '../../../service/resourceclient.service';
import { ResponseModel } from '../../../shared/models/common-data';
import { EmployeeModel } from '../../../shared/models/employee-data';
import { EmployeeDetailComponent } from '../employee-detail/employee-detail.component';

import * as moment from 'moment'; 

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styles: []
})
export class EmployeeComponent implements OnInit {

  pageTitle = "Employees Listing";
  addToolTip = "Add  new Company"; 
  rootPath="api/employees";

  displayedColumns: string[] = ['code','firstName','lastName','mobile','email','cardType','cardNumber','loginRole','toolbox'];

  dataSource = new MatTableDataSource<EmployeeModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    
    this.OnLoad(); 
  }

  OnLoad()
  {
    
    this.resourceClient.getDataInGet(this.rootPath).subscribe((data: EmployeeModel[])=>{
      this.dataSource = new MatTableDataSource<EmployeeModel>(data);
      this.dataSource.paginator = this.paginator;
    })  
    
  }
  OnAdd()
  {
    this.OnEdit('Add', {firstName: '',lastName: '',nationality: '',birthDate: null,gender: '',position: '',cardType: '',cardNumber: '',
    address1: '',address2: '',city: '',country: '',pincode: '',email: '',mobile: '',phone: '',loginRole: 'None',userpassword: ''
    });
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(EmployeeDetailComponent, {
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
          this.resourceClient.getDataInDelete(this.rootPath,result.data.master.code).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          });
          return; 
        }
        
        var aDate   = moment(result.data.master.birthDate, 'YYYY-MM-DD', true);

        if (aDate.isValid())
          result.data.master.birthDate = result.data.master.birthDate.format('YYYY-MM-DD') ;
        else
          result.data.master.birthDate = null; 
        
        if (result.data.master.userpassword.trim().length>0)
        {
          result.data.master.userpassword = btoa(result.data.master.userpassword);
        }

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
