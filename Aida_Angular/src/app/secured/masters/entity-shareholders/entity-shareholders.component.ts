import { Component, OnInit, ViewChild } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatSnackBar} from '@angular/material/snack-bar';
import { HttpUrlEncodingCodec } from '@angular/common/http';
import { Router} from '@angular/router';
import { ActivatedRoute} from '@angular/router';

import { ResourceClientService } from '../../../service/resourceclient.service';
import { ResponseModel } from '../../../shared/models/common-data';
import { EntityShareholderModel } from '../../../shared/models/client-profile-data';
import { EntityShareholderDetailsComponent } from '../entity-shareholder-details/entity-shareholder-details.component';


import * as moment from 'moment'; 

@Component({
  selector: 'app-entity-shareholders',
  templateUrl: './entity-shareholders.component.html',
  styles: []
})
export class EntityShareholdersComponent implements OnInit {

  codec = new HttpUrlEncodingCodec;
  rootPath="api/entity-shareholders";
  pageTitle='Entity Shareholders Listing';
  addToolTip = "Add  new Entity Shareholder";
  businessProfileId=0;
  businessProfileName='';

  displayedColumns: string[] = ['name','uen','incorpDate','phone','email','representativeName','status','toolbox'];
  
  dataSource = new MatTableDataSource<EntityShareholderModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;  
  
  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService, 
    private snackBar: MatSnackBar,private router: Router,private route: ActivatedRoute) { }

  ngOnInit() {
    
    this.route.params.subscribe(params => {

      this.businessProfileId = parseInt(params['businessProfileId']) ;
      this.businessProfileName = this.codec.decodeValue(params['businessProfileName']); 
      this.pageTitle = this.businessProfileName + '\'s  Entity Shareholders Listing';
      this.OnLoad();
    });
  }

  OnLoad()
  {
    
    this.resourceClient.getDataInGet(this.rootPath+'/'+this.businessProfileId).subscribe((data: EntityShareholderModel[])=>{
      this.dataSource = new MatTableDataSource<EntityShareholderModel>(data);
      this.dataSource.paginator = this.paginator;
    })  
    
  }
  OnAddEntity(data)
  {
    this.router.navigate(['secured/masters/entity-shareholders',data.id,data.name]);
  }


  OnAdd()
  {
    this.OnEdit('Add', {id: -1,name: '',uen: '',incorpDate: null,address: '',country: '',
    phone: '',email:'',status: '',tradingName:'',nature:'',representativeId:0,businessProfileId:this.businessProfileId });
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(EntityShareholderDetailsComponent, {
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

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }

}
