import { Component, OnInit, ViewChild,AfterViewInit } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import { MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatSnackBar} from '@angular/material/snack-bar';
import { MatTableExporterDirective } from 'mat-table-exporter';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { ResponseModel } from '../../../shared/models/common-data';
import { DocumentModel } from '../../../shared/models/document-data';
import { DocumentDetailComponent } from '../document-detail/document-detail.component';

@Component({ 
  selector: 'app-document',
  templateUrl: './document.component.html'
})
export class DocumentComponent implements OnInit, AfterViewInit {

  pageTitle = "Document Listing";
  addToolTip = "Add  new Document";
  filterCaption = "Search in Documents";
  rootPath="api/documents";

  displayedColumns: string[] = ['code','name','fileName','effectiveDate','serviceName','toolbox'];
  
  dataSource = new MatTableDataSource<DocumentModel>();

  //@ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('exporter',{ static: true}) exporter: MatTableExporterDirective;

  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService, 
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    
    this.OnLoad();
  }

  OnLoad()
  {
    
    this.resourceClient.getDataInGet(this.rootPath).subscribe((data: DocumentModel[])=>{
      this.dataSource.data = data ;
    })  
    
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;

  }

  OnAdd()
  {
    this.OnEdit('Add', {code: '',name: '',filename: '',effectdate:'',versionNo: '',status: '' });
  }
  OnEdit(option,paramData) {
    const dialogRef = this.dialog.open(DocumentDetailComponent, {
      width: '1200px',
      data:{action:option,data:paramData},disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        if(result.event==='Cancel')
          return;
          this.OnShowMessage('Add / Modify / Delete operations blocked now');  
        /*
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
        
        */
      }
      
    });

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

}
