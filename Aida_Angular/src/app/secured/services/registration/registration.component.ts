import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';
import { RegistrationOptionsComponent } from '../registration-options/registration-options.component';

import { ResponseModel,DropDownModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { SendFormModel, ServiceRegistrationDisplayModel,ServicesDefinitionModel } from '../../../shared/models/service-data';
import { ServicesSOPModel } from '../../../shared/models/service-data';
import { ServicesModule } from '../services.module';
import { SendFormComponent } from 'src/app/shared/components/send-form/send-form.component';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styles: []
})
export class RegistrationComponent implements OnInit {

  codec :HttpUrlEncodingCodec = new HttpUrlEncodingCodec;

  pageTitle = '';
  serviceCode='';
  service:ServicesDefinitionModel;

  businessProfileId='0';  
  companyNameList: DropDownModel[] = [];
  isPosted:boolean=false;
  canSendForm:boolean=false;

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

                                
  displayedColumns: string[] = ['serviceName','businessProfileName','officerName','executor','remarks','status','toolbox'];
  
  dataSource = new MatTableDataSource<ServiceRegistrationDisplayModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.serviceCode = params['serviceCode'];
      if(this.serviceCode==='registrations')
        this.canSendForm=true;

      this.businessProfileId = params['businessProfileId'];
      if (this.businessProfileId==null || this.businessProfileId==undefined)
        this.businessProfileId='0';
      
      this.resourceClient.getDataInGet('api/service-definitions/'+this.serviceCode).subscribe((data: ServicesDefinitionModel)=>{
        this.service = data;
        this.pageTitle =  this.service.name;
        if(parseInt(this.businessProfileId)>0)
        {
          if (this.service.hasOptionalDocument==='true')
            this.OnShowOption(this.businessProfileId);
          else 
            this.OnRegister(this.businessProfileId,'');
        }
      });

      
    });

    this.resourceClient.getDataInGet('api/masters/dropdown/business-profile').subscribe((data: DropDownModel[])=>{
      this.companyNameList = data;
    })

  }

  OnAddNewProfile()
  {
    this.router.navigate(['secured/masters/client-profile']);
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
  OnCreate(){
    var id = this.GetCompanyId();
    if(id>0)
    {
      if (this.service.hasOptionalDocument==='true')
        this.OnShowOption(id);
      else 
        this.OnRegister(id,'');
    }
    
  }

  OnSendForm()
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
  OnShowOption(id)
  {
    var dialogWidth='90%';
    if (this.serviceCode==='PostRegistration')
      dialogWidth='54%'
      
    const dialogRef = this.dialog.open(RegistrationOptionsComponent, {
      width: dialogWidth,
      data:this.serviceCode,
      disableClose: true 
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
        if(result.event==='Update')
        {
          /*
          var stepIds:string[]=[];
          result.data.forEach(model => {
            stepIds.push(model.stepId) ;
          });
          var steps:string = stepIds.join(',');
          */
         var steps:string = result.data;
          if (steps.length>0)
            this.OnRegister(id,steps);
        }

      }
      
    });

  }
  OnRegister(id,steps){
    this.resourceClient.getDataInPut('api/service-registration',
      {serviceCode:this.serviceCode,businessProfileId:id,sessionToken:'sss',stepIds:steps}).subscribe((data: ResponseModel)=>{
        this.OnShowMessage(data.message);
        this.isPosted=true;
        if (data.isSuccess)
        {
          this.resourceClient.getDataInGet('api/service-registration/'+this.serviceCode+'/'+id+'/New').subscribe((data: any[])=>{
            this.dataSource = new MatTableDataSource<ServiceRegistrationDisplayModel>(data);
            this.dataSource.paginator = this.paginator;
          })  
        }
    })
    
    
  }

  OnStart(data)
  {
    this.router.navigate(['secured/services/execution/'+data.serviceBusinessId+'/'+data.officerId]);
  }
  OnExport(exportType){

  }

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }

}
