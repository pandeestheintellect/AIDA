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

import { OfficersDetailComponent } from '../officers-detail/officers-detail.component';
import { ManagingDirectorsDetailComponent } from '../managing-directors-detail/managing-directors-detail.component';
import { SecretariesDetailComponent } from '../secretaries-detail/secretaries-detail.component';
import { ControllersIndividualsComponent } from '../controllers-individuals/controllers-individuals.component';

const moment = _moment;

export interface DataModel {
  id: number;
  entryDate: string;
  
  startDate: string;
  cessationDate: string;
  remarks1:String,
  remarks2:string,
  userRole: string;
  businessProfileId:number,  
  officerId:number,
  officerName: string;
}


@Component({
  selector: 'app-officers',
  templateUrl: './officers.component.html',
  styles: []
})
export class OfficersComponent implements OnInit {

  pageTitle = "Register of Directors";
  addToolTip = "Add  new Directors details";
  filterCaption = "Search in Directors details";
  customIcon: any ={ iconName: 'file-download', prefix: 'fas' };;
  customToolTip: string='Download register';

  userRole='';
  clientProfileId='0';
  officerId='0';
  lastColHeading='';

  companyNameList: DropDownModel[] = [];
  officerList: DropDownModel[] = [];
  identyTypeList: DropDownModel[] = [];

  rootPath="api/register/officers-registers";

  displayedColumns: string[] = ['entryDate','officerName','startDate','cessationDate','remarks1','toolbox'];
  
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
    this.resourceClient.getDataInGet('api/masters/dropdown/identity-type').subscribe((data: DropDownModel[])=>{
      this.identyTypeList = data;
    });

    this.route.params.subscribe(params => {
      this.clientProfileId = params['clientProfileId'] ;
      this.userRole = params['userrole'] ;
      if (this.userRole==='directors')
      {
        this.pageTitle = "Register of Directors";
        this.addToolTip = "Add new Directors details";
        this.filterCaption = "Search in Directors details";
        this.userRole = 'Directors';
        this.lastColHeading = 'Remarks 1';
      }
      else if (this.userRole==='managing-directors')
      {
        this.pageTitle = "Register of Managing Directors";
        this.addToolTip = "Add new Managing Directors details";
        this.filterCaption = "Search in Managing Directors details";
        this.userRole = 'Managing Directors';
        this.lastColHeading = 'Other Occupation';
      }
      else if (this.userRole==='secretaries')
      {
        this.pageTitle = "Register of Secretaries";
        this.addToolTip = "Add new Secretaries details";
        this.filterCaption = "Search in Secretaries details";
        this.userRole = 'Secretaries';
        this.lastColHeading = 'Other Occupation';
      }
      else if (this.userRole==='controllers-individuals')
      {
        this.pageTitle = "Register of Registrable Controllers (For Individuals)";
        this.addToolTip = "Add new Registrable Controllers details";
        this.filterCaption = "Search in Registrable Controllers";
        this.userRole = 'Controllers-Individuals';
        this.lastColHeading = 'Notice Sent On';
      }
      this.onLoadOfficer();
      this.OnLoad();
    });

    
  }
  onLoadOfficer()
  {
    this.resourceClient.getDataInGet('api/business-officers-dropdown/'+this.clientProfileId).subscribe((data: DropDownModel[])=>{
      this.officerList = data;
    });

  }
  OnLoad(){
    this.resourceClient.getDataInGet(this.rootPath+'/'+this.userRole+'/'+this.clientProfileId+'/'+this.officerId).subscribe((data: DataModel[])=>{
      this.dataSource = new MatTableDataSource<DataModel>(data);
      this.dataSource.paginator = this.paginator;
    })  
  }
  OnAdd()
  {
    this.OnEdit('Add',{id: 0,entryDate: null, startDate: null,cessationDate: null,
      remarks1:'',remarks2:'',businessProfileId:this.clientProfileId, officerId:this.officerId,officerName: ''});
  }
  OnEdit(option,paramData) {
    var dialogRef;

    if (this.userRole==='Directors')
    {
      dialogRef = this.dialog.open(OfficersDetailComponent, {
        width: '1200px',
        data:{action:option,data:paramData,officers:this.officerList,identityTypes:this.identyTypeList},disableClose: true
      });
      }
    else if (this.userRole==='Managing Directors')
    {
      dialogRef = this.dialog.open(ManagingDirectorsDetailComponent, {
        width: '1200px',
        data:{action:option,data:paramData,officers:this.officerList,identityTypes:this.identyTypeList},disableClose: true
      });
      }
    else if (this.userRole==='Secretaries')
    {
      dialogRef = this.dialog.open(SecretariesDetailComponent, {
        width: '1200px',
        data:{action:option,data:paramData,officers:this.officerList,identityTypes:this.identyTypeList},disableClose: true
      });
      }
      else if (this.userRole==='Controllers-Individuals')
      {
        dialogRef = this.dialog.open(ControllersIndividualsComponent, {
          width: '1200px',
          data:{action:option,data:paramData,officers:this.officerList,identityTypes:this.identyTypeList},disableClose: true
        });
        }
 
    
    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
        if(result.event==='Delete')
        {
          this.resourceClient.getDataInDelete(this.rootPath+'/'+this.userRole,result.data.id).subscribe((data: ResponseModel)=>{
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

          this.resourceClient.getDataInPost(this.rootPath+'/'+this.userRole,result.data).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          }) 
        }
        else 
        {

          this.resourceClient.getDataInPut(this.rootPath+'/'+this.userRole,result.data).subscribe((data: ResponseModel)=>{
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
    this.resourceClient.getDataInGet('api/register/download-officers-registers/'+this.userRole+'/'+this.clientProfileId+'/'+this.officerId).subscribe((data: any)=>{
      setTimeout(() =>  window.open(this.resourceClient.REST_API_SERVER + 'api/register-download/'+data,'_blank'), 1000);

    });
  }
} 