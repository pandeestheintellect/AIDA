import { Component, OnInit, ViewChild } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatSnackBar} from '@angular/material/snack-bar';
import { Router} from '@angular/router';

import { ResourceClientService } from '../../../service/resourceclient.service';
import { ResponseModel } from '../../../shared/models/common-data';
import { ClientProfileModel,ClientProfileActivity } from '../../../shared/models/client-profile-data';
import { ClientProfileDetailComponent } from '../client-profile-detail/client-profile-detail.component';
import { ClientActivityComponent } from '../client-activity/client-activity.component';

import * as moment from 'moment'; 
import { SendFormModel } from 'src/app/shared/models/service-data';
import { SendFormComponent } from 'src/app/shared/components/send-form/send-form.component';

@Component({
  selector: 'app-client-profile',
  templateUrl: './client-profile.component.html',
  styles: []
})
export class ClientProfileComponent implements OnInit {

  rootPath="api/business-profile";
  pageTitle='Client Profile Listing';
  addToolTip = "Add  new Client Profile";
  customIcon: any ={ iconName: 'envelope', prefix: 'fas' };;
  customToolTip: string='Send Initial Document in mail';

  
  documentList:SendFormModel={businessProfileId: 0,name: '',email:'',mobile:'',message: '',documentType:'Registration',
        documentNames:[{stepId:1, documentName:'Client Profile',filePath:'Client Profile.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
        {stepId:2, documentName:'Officers Profile',filePath:'Officers Profile.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
        {stepId:2, documentName:'Entity Shareholder Profile',filePath:'Entity Shareholder Profile.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
        {stepId:2, documentName:'ABS PDPA Consent-Customer',filePath:'ABS PDPA Consent-Customer V-20181220.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
        {stepId:2, documentName:'ABS CDD-KYC Form',filePath:'ABS CDD-KYC Form V-20181112.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
        {stepId:2, documentName:'ABS CDD-CAF Form',filePath:'ABS CDD-CAF Form V-20181112 ACRA V-2.3.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
        {stepId:2, documentName:'ABS Share Capital Indemnity',filePath:'ABS Share Capital Indemnity.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
        {stepId:2, documentName:'Incorp-7 ACRA Form-45 For Director',filePath:'Incorp-7 ACRA Form-45 For Director.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''},
        {stepId:2, documentName:'Incorp-8 ACRA Form-45B For Secretary',filePath:'Incorp-8 ACRA Form-45B For Secretary.pdf',code:'',dependencyStepNo:0,executor:'',remarks:'',stepNo:0,versionNo:''}]
                      }

                      
  displayedColumns: string[] = ['name','uen','incorpDate','mobile','email','clientType','status','toolbox'];
  
  dataSource = new MatTableDataSource<ClientProfileModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;  
  
  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService, 
    private snackBar: MatSnackBar,private router: Router) { }

  ngOnInit() {
    
    this.OnLoad();
  }

  OnLoad()
  {
    
    this.resourceClient.getDataInGet(this.rootPath).subscribe((data: ClientProfileModel[])=>{
      this.dataSource = new MatTableDataSource<ClientProfileModel>(data);
      this.dataSource.paginator = this.paginator;
    })  
    
  }
  OnAddOfficer(data)
  {
    this.router.navigate(['secured/masters/client-officer',data.id,data.name]);
  }
  OnAddEntity(data)
  {
    this.router.navigate(['secured/masters/entity-shareholders',data.id,data.name]);
  }
  OnAddActivity(paramData)
  {
    const dialogRef = this.dialog.open(ClientActivityComponent, {
      width: '1200px',
      data:paramData.id,disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
  
          this.resourceClient.getDataInPut('api/business-activity',result.data).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          }) 
       
      }
      
    });
  }
  OnRegisters(param,paramData)
  {
    this.router.navigate(['secured/registers',param,paramData.id]);
  }

  OnAdd()
  {
    this.OnEdit('Add', {id: -1,name: '',uen: '',incorpDate: null,address1: '',address2:'',city: '',country: '',
    pincode: '',mobile: '',email:'',industryType: '',status: '',statusDate: null,tradingName:'',phone:'',nature:'',clientType:'PRIVATE LIMITED COMPANY'});
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(ClientProfileDetailComponent, {
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
          this.resourceClient.getDataInDelete(this.rootPath,result.data.id).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          });
          return 
        }
  
        /*
        if (result.data.uen.length<2)
        {
          this.OnShowMessage('Please enter valid UEN');
          return;
        }
        */
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
  OnReport(id)
  {
    
  }
  OnExport(exportType){

  }
  OnCustom()
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
  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }

}
