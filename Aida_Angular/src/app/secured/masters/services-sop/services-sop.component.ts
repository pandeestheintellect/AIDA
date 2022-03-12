import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator} from '@angular/material/paginator';
import { MatTableDataSource} from '@angular/material/table';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import { HttpUrlEncodingCodec } from '@angular/common/http';
import { MatSnackBar} from '@angular/material/snack-bar';
import { ActivatedRoute} from '@angular/router';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { ServicesSOPDetailComponent } from '../services-sop-detail/services-sop-detail.component';

import { ServicesSOPModel } from '../../../shared/models/service-data';
import { ResponseModel } from '../../../shared/models/common-data';


@Component({
  selector: 'app-services-sop',
  templateUrl: './services-sop.component.html',
  styles: []
})
export class ServicesSOPComponent implements OnInit {

  pageTitle="Service Definition";
  addToolTip = "Add  new Service Definition";
  rootPath="api/service-sop";
  serviceCode='';
  displayedColumns: string[] = ['stepNo','executor','remarks','documentName','versionNo','toolbox'];
  
  dataSource = new MatTableDataSource<ServicesSOPModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;


  constructor(public dialog: MatDialog,private resourceClient: ResourceClientService,
    private snackBar: MatSnackBar,private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {

      this.pageTitle = params['serviceCode'] + ' Service';
      this.serviceCode =  params['serviceCode']
    });
    this.OnLoad();
  }
  OnLoad()
  {
    this.resourceClient.getDataInGet(this.rootPath+'/'+this.serviceCode).subscribe((data: any[])=>{
      this.dataSource = new MatTableDataSource<ServicesSOPModel>(data);
    })  
    this.dataSource.paginator = this.paginator;
  }
  OnAdd()
  {
    this.OnEdit('Add', 'ALL');
  }
  OnEdit(option,role) {
    const dialogRef = this.dialog.open(ServicesSOPDetailComponent, {
      width: '90%',
      data:{action:option,executor:role,serviceCode:this.serviceCode}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
        if(result.event==='Add' || result.event==='Update')
        {
          this.resourceClient.getDataInPut(this.rootPath+'/'+this.serviceCode+'/'+result.data.executor,result.data.document).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          }) 
        }
        
        else if(result.event==='Delete')
        {
          this.resourceClient.getDataInDelete(this.rootPath,result.data.serviceCode).subscribe((data: ResponseModel)=>{
            this.OnShowMessage(data.message);
            this.OnLoad();
          }) 
        }
        
      }
      
    });

  }
  OnExport(exportType){
    this.OnShowMessage(exportType);
  }
  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }

}
