import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator} from '@angular/material/paginator';
import { MatTableDataSource} from '@angular/material/table';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { ServicesDefinitionModel } from '../../../shared/models/service-data';
import { ServiceDefinitionDetailComponent } from '../service-definition-detail/service-definition-detail.component';
import { ResponseModel } from '../../../shared/models/common-data';
import { ServiceDocumentsComponent } from '../service-documents/service-documents.component';

@Component({
  selector: 'app-service-definition',
  templateUrl: './service-definition.component.html',
  styles: []
})
export class ServiceDefinitionComponent implements OnInit {

  pageTitle="Service Definition";
  addToolTip = "Add  new Service Definition";
  rootPath="api/service-definitions";

  displayedColumns: string[] = ['code','name','remarks','hasOptionalDocument','toolbox'];
  
  dataSource = new MatTableDataSource<ServicesDefinitionModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  constructor(public dialog: MatDialog,private resourceClient: ResourceClientService,
    private snackBar: MatSnackBar,private router: Router) { }

  ngOnInit() {
    
    this.OnLoad();
  }
  OnLoad()
  {
    this.resourceClient.getDataInGet(this.rootPath).subscribe((data: any[])=>{
      this.dataSource = new MatTableDataSource<ServicesDefinitionModel>(data);
    })  
    this.dataSource.paginator = this.paginator;
  }
  OnAddDocument(paramData){
    const dialogRef = this.dialog.open(ServiceDocumentsComponent, {
      width: '60%',
      data:{value:paramData.code,text:paramData.name},disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
         if(result.event==='Update')
        {
          this.resourceClient.getDataInPut('api/service-definitions-documents',result.data).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          }) 
        }
      }
      
    });
  }
  OnNewService(data){
    this.router.navigate(['secured/masters/service-sop',data.code]);
  }
  OnAdd()
  {
    this.OnEdit('Add', {companyId: -1,companyName: '',uen: '',incorpDate: new Date(),address1: '',address2:'',city: '',country: '',
    pincode: '',mobile: '',email:''});
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(ServiceDefinitionDetailComponent, {
      width: '90%',
      data:{action:option,data:paramData},disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
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
        else if(result.event==='Delete')
        {
          this.resourceClient.getDataInDelete(this.rootPath,result.data.code).subscribe((data: ResponseModel)=>{
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
