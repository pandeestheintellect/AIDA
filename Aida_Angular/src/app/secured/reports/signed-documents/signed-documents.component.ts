import { Component, OnInit, ViewChild,AfterViewInit } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import { MatSort} from '@angular/material/sort';


import {ActivatedRoute,Router} from '@angular/router';
import {MatTableDataSource} from '@angular/material/table'; 
import { MatTableExporterDirective } from 'mat-table-exporter';

import { DialogDataModel,ResponseModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { ServiceRegistrationDisplayModel } from '../../../shared/models/service-data';

@Component({
  selector: 'app-signed-documents',
  template: `
<div class="mat-elevation-z4 app-page">

  <app-page-toolbar [title]="pageTitle" [addNew]="false" (OnFilter)="OnFilter($event)" 
      [filterCaption]="filterCaption" [hasFilter]="true" (OnExport)="OnExport($event)"></app-page-toolbar>

      <table class="table" mat-table [dataSource]="dataSource" *ngIf="dataSource.data.length>0" matTableExporter #exporter="matTableExporter"> 
      <ng-container matColumnDef="created">
          <th mat-header-cell *matHeaderCellDef> Date </th>
          <td mat-cell *matCellDef="let element"> {{element.created}} </td>
        </ng-container>
      <ng-container matColumnDef="serviceName">
            <th mat-header-cell *matHeaderCellDef> Service Name </th>
            <td mat-cell *matCellDef="let element"> {{element.serviceName}} </td>
        </ng-container>
        <!-- Code Column -->

      <!-- Name Column -->
      <ng-container matColumnDef="officerName">
          <th mat-header-cell *matHeaderCellDef> Officer Name </th>
          <td mat-cell *matCellDef="let element"> {{element.officerName}} </td>
      </ng-container>
          <!-- Remarks Column -->
          <ng-container matColumnDef="documentName">
            <th mat-header-cell *matHeaderCellDef> Document Name </th>
            <td mat-cell *matCellDef="let element"> {{element.documentName}} </td>
          </ng-container>
          <ng-container matColumnDef="remarks">
            <th mat-header-cell *matHeaderCellDef> Remarks </th>
            <td mat-cell *matCellDef="let element"> {{element.remarks}} </td>
          </ng-container>

 
        <!-- Get Details --> 
      <ng-container matColumnDef="toolbox">
          <th mat-header-cell *matHeaderCellDef width ="40"> </th>
          <td mat-cell *matCellDef="let element"> 
              <mat-button-toggle-group #group="matButtonToggleGroup" >
                <mat-button-toggle value="center" matTooltip="View the document" 
                  matTooltipClass="snackBarBackgroundColor" (click)="OnDownload(3,element)">
                  <fa-icon [icon]="['fas', 'download']"></fa-icon>
                </mat-button-toggle>
                  
                </mat-button-toggle-group>


          </td>
        </ng-container>
  
        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
      </table>

      <mat-paginator [pageSize]="8" [pageSizeOptions]="[8, 20,40,80]" style="padding-bottom: 30px;">
        </mat-paginator>
        

</div>

  `,
  styles: []
})
export class SignedDocumentsComponent implements OnInit, AfterViewInit {
 
  pageTitle = "Documents";
  addToolTip = "Add  new Document";
  filterCaption = "Search in Documents";

  businessProfileId:number=0;

  displayedColumns: string[] = ['created','serviceName','officerName','documentName','toolbox'];
  
  dataSource = new MatTableDataSource<ServiceRegistrationDisplayModel>();
  documents: ServiceRegistrationDisplayModel[] = [];
  
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('exporter',{ static: true}) exporter: MatTableExporterDirective;

  constructor(private resourceClient: ResourceClientService,private route: ActivatedRoute) { 
    this.route.params.subscribe(params => {
    this.businessProfileId = params['businessProfileId'];
    this.pageTitle = 'Signed documents for ' + params['businessProfileName'];
    });
  }
    

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/service-registration/A/'+this.businessProfileId+'/Completed').subscribe((data: any[])=>{
      this.dataSource = new MatTableDataSource<ServiceRegistrationDisplayModel>(data); 
      this.documents = data;
      
    }) 
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;

  }
  OnDownload(id,data)
  {
    if ( id===3 && data.downloadedFileName!=='NotDownloaded')
    {
      window.open(this.resourceClient.REST_API_SERVER + 'api/file-download/'+data.downloadedFileName,'_blank') ;
    }
    
  }
  OnExport(exportType){
    if (exportType===1)
      this.exporter.exportTable('xlsx', {fileName:'AIDAExport', sheet: 'sheet_name', Props: {Author: 'AIDA'}})
  }
  OnFilter(filterText:string){
    this.dataSource.filter = filterText.trim().toLocaleLowerCase();
  }
}
