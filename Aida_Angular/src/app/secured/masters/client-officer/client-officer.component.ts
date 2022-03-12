import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator} from '@angular/material/paginator';
import { MatTableDataSource} from '@angular/material/table';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { HttpUrlEncodingCodec } from '@angular/common/http';
import { MatSnackBar} from '@angular/material/snack-bar';
import { ActivatedRoute} from '@angular/router';


import { ResourceClientService } from '../../../service/resourceclient.service';
import { ResponseModel } from '../../../shared/models/common-data';
import { ClientProfileModel } from '../../../shared/models/client-profile-data';
import { ClientOfficerModel } from '../../../shared/models/client-officer-data';
import { ClientOfficerDetailComponent } from '../client-officer-detail/client-officer-detail.component';
import { ClientMyinfoIntroComponent } from '../client-myinfo-intro/client-myinfo-intro.component';

import * as moment from 'moment'; 

@Component({
  selector: 'app-client-officer',
  templateUrl: './client-officer.component.html',
  styles: []
})
export class ClientOfficerComponent implements OnInit {

  codec = new HttpUrlEncodingCodec;
  pageTitle='';
  addToolTip = "Add  new Officer";
  rootPath="api/business-officers";
  businessProfileId=0;
  businessProfileName='';
  businessProfileType='';
  displayedColumns: string[] = ['name','address','userRole','nationality','mobile','email','toolbox'];
  
  dataSource = new MatTableDataSource<ClientOfficerModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  
  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,
    private snackBar: MatSnackBar,private route: ActivatedRoute) { }

  ngOnInit() {
    
    this.route.params.subscribe(params => {

      this.businessProfileId = parseInt(params['businessProfileId']) ;
      this.businessProfileName = this.codec.decodeValue(params['businessProfileName']); 
      this.pageTitle = this.businessProfileName + '\'s  Officers Listing';
      this.OnLoad();
    });
    
  }

  OnLoad()
  {
    this.resourceClient.getDataInGet('api/business-profile/'+this.businessProfileId).subscribe((data: ClientProfileModel)=>{
      this.businessProfileType = data.clientType;
    })  

    this.resourceClient.getDataInGet(this.rootPath+'/'+this.businessProfileId).subscribe((data: ClientOfficerModel[])=>{
      this.dataSource = new MatTableDataSource<ClientOfficerModel>(data);
      this.dataSource.paginator = this.paginator;
    })  
   
  }

  OnAdd()
  {
    const dialogRef = this.dialog.open(ClientMyinfoIntroComponent, {
      width: '800px',
      data:'',disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
        if(result.event==='Form')
        {
          this.OnEdit('Add', {officerId: -1,businessProfileId: this.businessProfileId,name: '',address: '',nationality:'' ,birthDate: null,
            birthCountry:'',position: '',userRole: '',mobile: '',email:'',nricNo:'',nricIssueDate:null,finNo:'',finIssueDate:null,
            finExpiryDate:null,passportNo:'',passportIssueDate:null,passportIssuePlace:'',passportExpiryDate:null,
            sex:'',phone:'',birthPlace:'',joinDate:null,passportIssueCountry:'',numberOfShares:0,myInfoRequestId:0}); 
        }
        else if(result.event==='MyInfo')
        {

          this.resourceClient.getDataInGet('api/myinfo-authorise/'+this.businessProfileId).subscribe((data:string)=>{
            window.open(data,'_blank');
            var keyvalue=data.split('&');
            for(var i=0;i<keyvalue.length;i++)
            {
              if (keyvalue[i].indexOf('state=')>=0)
              {
                keyvalue=keyvalue[i].split('=');
                setTimeout(this.OnMyInfoTimer,1000, this,keyvalue[1]);
                break;
              }
                
            }
          })  

          
        }
        else 
        {

          this.resourceClient.getDataInGet('api/myinfo-authorise/'+this.businessProfileId+'/'+btoa(result.event)).subscribe((data:ResponseModel)=>{
            this.OnShowMessage(data.message);
          })  

          
        }
        
      }
      
    });

  }
  OnMyInfoTimer(that:any, state:string)
  {
    that.resourceClient.getDataInGet('api/myinfo/'+state).subscribe((data: any[])=>{
      if(data===null)
      {
        setTimeout(that.OnMyInfoTimer,1000,that,state);
      }
      else
      {
        that.OnEdit('Update',data);
      }
    })  
  }
  OnEdit(option,paramData) {

    if(option==='Delete' && paramData.userRole==='Authorised Representative')
    {
      this.OnShowMessage('Authorised Representative can not be deleted.');
      return;
    }

    const dialogRef = this.dialog.open(ClientOfficerDetailComponent, {
      width: '1300px',
      data:{action:option+'~'+this.businessProfileType,data:paramData},disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
        
          if(result.event==='Delete')
        {
          this.resourceClient.getDataInDelete(this.rootPath,result.data.officerId).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          }) 
          return;
        }          
        
        result.data.birthDate = this.GetFormatedDate(result.data.birthDate);
        result.data.nricIssueDate = this.GetFormatedDate(result.data.nricIssueDate);
        result.data.nricExpiryDate = this.GetFormatedDate(result.data.nricExpiryDate);
        result.data.finIssueDate = this.GetFormatedDate(result.data.finIssueDate);
        result.data.finExpiryDate = this.GetFormatedDate(result.data.finExpiryDate);
        result.data.passportIssueDate = this.GetFormatedDate(result.data.passportIssueDate);
        result.data.passportExpiryDate = this.GetFormatedDate(result.data.passportExpiryDate);
        result.data.joinDate = this.GetFormatedDate(result.data.joinDate);
        
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
  GetFormatedDate(datestring)
  {
    var aDate   = moment(datestring, 'YYYY-MM-DD', true);

    if (aDate.isValid())
      return datestring.format('YYYY-MM-DD') ; 
    else
      return null; 

  }
}
