import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table'; 
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';

import { ResponseModel,DropDownModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { ServicesDefinitionModel } from '../../../shared/models/service-data';
import { ServiceRegistrationDisplayModel } from '../../../shared/models/service-data';
import { SigningComponent } from '../signing/signing.component';
import { UploadComponent } from '../upload/upload.component';
import { updateLocale } from 'moment';

@Component({
  selector: 'app-listing',
  templateUrl: './listing.component.html',
  styles: []
})
export class ListingComponent implements OnInit {

  codec :HttpUrlEncodingCodec = new HttpUrlEncodingCodec;

  pageTitle = 'Service Listing';
  addToolTip = "Add  new Service";
  serviceCode='';
  status='';
  businessProfileId='';  
  serviceBusinessId='0';
  servicesList: DropDownModel[] = [];
  companyNameList: DropDownModel[] = [];
  statusList: DropDownModel[] = [];

  displayedColumns: string[] = ['businessProfileName','officerName','executor','remarks','status','toolbox'];
  
  dataSource = new MatTableDataSource<ServiceRegistrationDisplayModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.serviceCode = params['serviceCode'];
      if (this.serviceCode==null || this.serviceCode==undefined)
        this.serviceCode='';
      this.businessProfileId = params['businessProfileId'];
      if (this.businessProfileId==null || this.businessProfileId==undefined)
        this.businessProfileId='';

      this.serviceBusinessId = params['serviceBusinessId'];  
      if (this.serviceBusinessId==null || this.serviceBusinessId==undefined)
        this.serviceBusinessId='';  
        

      this.status = params['status'];

      if (this.status==null || this.status==undefined)
        this.status='';

      if (this.serviceCode.length>0 && this.businessProfileId.length>0 && this.status.length>0)
        this.OnGet();

    });

    this.resourceClient.getDataInGet('api/masters/dropdown/business-profile').subscribe((data: DropDownModel[])=>{
      this.companyNameList = data;
    });
    this.resourceClient.getDataInGet('api/masters/dropdown/services').subscribe((data: DropDownModel[])=>{
      this.servicesList = data;
    });
    this.resourceClient.getDataInGet('api/masters/dropdown/services-status').subscribe((data: DropDownModel[])=>{
      this.statusList = data;
    });
/*
    this.resourceClient.getDataInGet('api/service-definitions/'+this.serviceCode).subscribe((data: ServicesDefinitionModel)=>{
      this.pageTitle =  data.name;

    });
*/
  }

  
  GetCompanyId():number {
    var id = parseInt(this.businessProfileId);
    if (id>0)
    {
      return id;
    }
    else
    {
      this.OnShowMessage("Please choose a business profile to continue");
      return 0;
    }
      
  }
  OnGet()
  {
    if (this.serviceCode.length>0 || this.businessProfileId.length>0 || this.status.length>0)
    {
      if (this.businessProfileId=='')
        this.businessProfileId='0';
      if (this.status=='')
        this.status='A';
      if (this.serviceCode=='')
        this.serviceCode='A';
      
      if (this.serviceBusinessId=='')
        this.serviceBusinessId='0';

      this.resourceClient.getDataInGet('api/service-registration/'+this.serviceCode+'/'+this.businessProfileId+'/'+this.status+'/'+this.serviceBusinessId)
        .subscribe((data: any[])=>{
          this.dataSource = new MatTableDataSource<ServiceRegistrationDisplayModel>(data);
          this.dataSource.paginator = this.paginator;
          if (data.length>0)
            this.pageTitle =  data[0].serviceName + ' for ' + data[0].businessProfileName;
          else
          this.pageTitle ='Service Listing';

          this.serviceBusinessId='';
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
            this.dataSource = new MatTableDataSource<ServiceRegistrationDisplayModel>(data);
            this.dataSource.paginator = this.paginator;
          })  
      })
     
    }
  }
  OnDownload(paramData)
  {
    this.router.navigate(['secured/masters/download-forms/'+this.serviceCode]); 
  }
  OnUpload(paramData)
  {
    var uploadData:ServiceRegistrationDisplayModel = paramData;
    uploadData.serviceBusinessId = parseInt(this.businessProfileId) ;
    uploadData.serviceName = this.serviceCode;

    const dialogRef = this.dialog.open(UploadComponent, {
      width: '600px',
      data:{action:'Signing',data:paramData},
      disableClose: true 
    });
  }
  OnInvite(data)
  {
    var postdata:any ={
      serviceBusinessId : data.serviceBusinessId,
      officerStepIds: data.officerId+''
    };
    
    this.resourceClient.getDataInPost('api/service-execution/invite',postdata).subscribe((data: ResponseModel)=>{
     
      this.OnShowMessage(data.message);
    }) 
  }
  OnEdit(data)
  {
    this.router.navigate(['secured/services/execution/'+data.serviceBusinessId+'/'+data.officerId]); 
  }
  OnSignature(paramData)
  {
    const dialogRef = this.dialog.open(SigningComponent, {
      width: '1200px',
      data:{action:'Signing',data:paramData},
      disableClose: true 
    });

  }
  OnExport(exportType){

  }

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }
}