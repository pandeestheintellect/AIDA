import { Component, OnInit ,ViewChild} from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';
import { Validators } from "@angular/forms";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { FieldConfig } from "../../../shared/components/field.interface";
import { DynamicFormComponent } from "../../../shared/components/dynamic-form/dynamic-form.component";
import {ActivatedRoute,Router} from '@angular/router';
import {CdkStepper} from '@angular/cdk/stepper/stepper';
import { MatSnackBar} from '@angular/material/snack-bar';

import { ImportDocumentsComponent } from '../../masters/import-documents/import-documents.component';
import { ResponseModel,DropDownModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { SpinnerOverlayService} from  '../../../service/spinner-overlay.service';
import { PreviewComponent } from 'src/app/shared/components/preview/preview.component';
import { WebframeComponent } from 'src/app/shared/components/webframe/webframe.component';
import { MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { APP_DATE_FORMATS } from '../../../shared/format-datepicker';



import * as _moment from 'moment';

const moment = _moment;


export interface ServicesEntryModel {
  serviceBusinessId:number;
  officerStepId:number;
  documentName: string;
  serviceCode:string;
  serviceName: string;
  businessProfileName:string;
  officerName: string;
  officerRole: string;
  status:string;
  generatedFileName:string;
  fields: FieldConfig[];
}

export interface ServicesEntrySaveModel {
  serviceBusinessId:number;
  officerStepId:number;
  elements:string;
}

@Component({
  selector: 'app-execution',
  templateUrl: './execution.component.html',
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS}
  ]
})
export class ExecutionComponent implements OnInit {

  @ViewChild('dynaForm') form: DynamicFormComponent;
  @ViewChild('stepper') stepper: CdkStepper;

  pageTitle='';
  rootPath='api/service-execution';
  currentStep:number = -1;
  stepName='';
  stepRole='';
  nextCaption:string='';
  action1Caption:string='';
  action2Caption:string='';
  action3Caption:string='';
  action1Visible:boolean=false;
  action2Visible:boolean=false;
  action3Visible:boolean=false;
  importVisible:boolean=true;
  fillVisible:boolean=true;

  
  isDocumentOpening:boolean=false;
  serviceBusinessId:number=0; 
  officerId:number=0; 
  steps: ServicesEntryModel[] = [];

  regConfig: FieldConfig[] = [];


  constructor(public dialog: MatDialog,private resourceClient: ResourceClientService,private route: ActivatedRoute,
    private router: Router,private snackBar: MatSnackBar ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.serviceBusinessId = parseInt( params['serviceBusinessId']);
      this.officerId = parseInt( params['officerId']);
      if (this.serviceBusinessId>0)
      {
        this.OnLoad();
      }
      else
        this.OnShowMessage("Please choose a business profile to continue"); 
 
    });
    
  }
  OnLoad()
  {
    this.resourceClient.getDataInGet( this.rootPath +'/'+this.serviceBusinessId+'/'+this.officerId).subscribe((data: ServicesEntryModel[])=>{
     
      /*
      data.forEach(function (value:ServicesEntryModel) {
        value.fields.forEach(function (field:FieldConfig) {
          if (field.type==='date')
            field.value = moment(field.value,'DD/MM/YYYY');
      });

      });
      */
      this.steps = data;

      if (data.length>0)
      {
        this.pageTitle = data[0].serviceName + ' for ' + data[0].businessProfileName;
      }
      this.currentStep = 0;
      
      //this.OnLoadWindow();
      this.stepper.reset();
    })
  }
  OnStepChange(event: any): void {
    this.currentStep = event.selectedIndex;
    this.OnLoadWindow();     
  }

  OnPrevious(){
    this.stepper.previous();
  }

  OnNext(){
    if (this.currentStep!==this.steps.length-1)
    this.stepper.next(); 
    else
    this.router.navigate(['secured/services/clients/'+this.steps[this.currentStep].serviceCode.toLowerCase() ]); 
  }

  OnLoadWindow()
  {
    this.stepName = this.steps[this.currentStep].documentName ;

    if (this.currentStep!==this.steps.length-1)
      this.nextCaption ='Next';
    else
      this.nextCaption ='Close';

    this.importVisible= true;
    this.fillVisible= true;

    if (this.steps[this.currentStep].status==='New' || this.steps[this.currentStep].status==='In-Progress')
    {
      this.stepRole = this.steps[this.currentStep].officerRole + ' ' + this.steps[this.currentStep].officerName + ' ' + ' to fill following details';
      this.action1Caption='To Approve';
      this.action1Visible = true;
      this.action2Visible = false;
      this.action3Visible = true;

      if (this.steps[this.currentStep].fields.length===0)
      {
        this.isDocumentOpening=true;
        this.OnShowMessage('Opening form ...' );
        var postdata:ServicesEntrySaveModel ={
          serviceBusinessId : this.steps[this.currentStep].serviceBusinessId,
          officerStepId: this.steps[this.currentStep].officerStepId,
          elements: JSON.stringify({})
        };
        
        this.resourceClient.getDataInPost(this.rootPath,postdata).subscribe((data: ResponseModel)=>{
          
          const dialogRef = this.dialog.open(WebframeComponent, {
            width: '850px',height: '90%',
            data:{serviceBusinessId:this.steps[this.currentStep].serviceBusinessId,officerStepId:this.steps[this.currentStep].officerStepId},disableClose: true
          });
  
          dialogRef.afterClosed().subscribe(result => {
            this.isDocumentOpening=false;
          });
        }) 
  
      }
    

    }
    else{
      this.stepRole = this.steps[this.currentStep].officerRole + ' ' + this.steps[this.currentStep].officerName + ' ' + 
        ' filled details, status is ' + this.steps[this.currentStep].status;
      if (this.steps[this.currentStep].status==='Prepared')
      {
        this.action1Caption='Approve';
        this.action2Caption='Reject';
        this.action1Visible = true;
        this.action2Visible = true;
        this.action3Visible = true;
      }
      else if (this.steps[this.currentStep].status==='Approved')
      {
        this.action1Caption='To Sign';
        this.action1Visible = true;
        this.action2Visible = false;
        this.action3Visible = true;
      }
      else if (this.steps[this.currentStep].status==='Signing')
      {
        this.action1Visible = false;
        this.action2Visible = false;
        this.action3Visible = true;
      }
      else
      {
        this.action1Visible = false;
        this.action2Visible = false;
        this.action3Visible = false;
        this.importVisible= false;
        this.fillVisible= false;
        //this.form.onDisable();
      }

    }
  }

  OnAction(id)
  {
    var actionString:string='Terminate';
    if(id===1)
      actionString=this.action1Caption;
    else if(id===2)
      actionString=this.action2Caption;
    
    if (actionString!=='Terminate')
    {
      if (actionString==='To Approve')
        actionString='Prepared'
      else if (actionString==='Approve')
        actionString='Approved'
      else if (actionString==='Reject')
        actionString='In-Progress'
      else if (actionString==='To Sign')
        actionString='Signing'
    
    }

    var postdata:ServicesEntrySaveModel ={
      serviceBusinessId : this.steps[this.currentStep].serviceBusinessId,
      officerStepId: this.steps[this.currentStep].officerStepId,
      elements: actionString
    };
    
    this.resourceClient.getDataInPut(this.rootPath,postdata).subscribe((data: ResponseModel)=>{
     /* if (actionString==='In-Progress')
      {
        this.OnLoad();
      }
      else {
        this.steps[this.currentStep].status=actionString;
        this.OnLoadWindow();

      }
*/
      this.steps[this.currentStep].status=actionString;
      this.OnLoadWindow();
      this.OnShowMessage(data.message );
      
    }) 

  }
  OnDownload(id:number) 
  {
    var fileType='api/form-preview-pdf/';
    if (id===2)
      fileType="api/form-pdf/";

    if (id===2 && !(this.steps[this.currentStep].status==='New' || this.steps[this.currentStep].status==='In-Progress'))
    {
      this.OnShowMessage('Document in '+ this.steps[this.currentStep].status + ' status can not be edited');
      return;
    }
    this.isDocumentOpening=true;
    this.resourceClient.getDataInGet(fileType+this.steps[this.currentStep].serviceBusinessId+'/'
      +this.steps[this.currentStep].officerStepId).subscribe((data: any)=>{
        
        window.open(this.resourceClient.REST_API_SERVER + 'api/file-download/'+data,'_blank') ;
        this.isDocumentOpening=false;
      });
  }
  OnSubmit(value: any) {
    if (this.steps[this.currentStep].status==='New' || this.steps[this.currentStep].status==='In-Progress')
    {
      this.isDocumentOpening=true;
      var postdata:ServicesEntrySaveModel ={
        serviceBusinessId : this.steps[this.currentStep].serviceBusinessId,
        officerStepId: this.steps[this.currentStep].officerStepId,
        elements: JSON.stringify(value)
      };
      
      this.resourceClient.getDataInPost(this.rootPath,postdata).subscribe((data: ResponseModel)=>{
        this.OnShowMessage(data.message);
        this.OnFill();
      }) 
    }
    else
    {
      this.OnShowMessage('Step in '+ this.steps[this.currentStep].status + ' status can not be saved');
    }
    
  }
  OnPreview(){
    /*
    this.resourceClient.getDataInGet(this.rootPath+'/preview/'+this.steps[this.currentStep].serviceBusinessId+'/'
            +this.steps[this.currentStep].officerStepId).subscribe((data: ResponseModel)=>{
      this.OnShowMessage(data.message);
      
    }) 
    */
    
   const dialogRef = this.dialog.open(PreviewComponent, {
    width: '850px',height: '90%',
    data:{serviceBusinessId:this.steps[this.currentStep].serviceBusinessId,officerStepId:this.steps[this.currentStep].officerStepId},disableClose: true
  });
  
  
  }

  OnFill(){

    if (this.steps[this.currentStep].status==='New' || this.steps[this.currentStep].status==='In-Progress')
    {
      this.isDocumentOpening=true;
      const dialogRef = this.dialog.open(WebframeComponent, {
        width: '850px',height: '90%',
        data:{serviceBusinessId:this.steps[this.currentStep].serviceBusinessId,officerStepId:this.steps[this.currentStep].officerStepId},disableClose: true
      });
      
      dialogRef.afterClosed().subscribe(result => {
        this.isDocumentOpening=false;
      });
    }
    else
    {
      this.OnShowMessage('Step in '+ this.steps[this.currentStep].status + ' status can not be filled');
    }

  }

  OnImport()
  {
    if (this.steps[this.currentStep].status==='New' || this.steps[this.currentStep].status==='In-Progress')
    {
      this.isDocumentOpening=true;
      const dialogRef = this.dialog.open(ImportDocumentsComponent, {
        width: '600px',
        data:{action:'Import',data:'Registration'},
        disableClose: true  
      });
  
      dialogRef.afterClosed().subscribe(result => 
      {
        this.isDocumentOpening=false;
        if (result)
        {
          if(result.event==='Upload')
          {
            this.resourceClient.getDataInGet('api/import-registration/'+this.steps[this.currentStep].serviceBusinessId+'/'+
              this.steps[this.currentStep].officerStepId+'/'+btoa(result.data)).subscribe((data: any)=>{
              this.OnFill();
            });
   
          }
        }
      });
  
    }
    else
    {
      this.OnShowMessage('Step in '+ this.steps[this.currentStep].status + ' status can not be imported');
    }

  }

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 3500,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }
}