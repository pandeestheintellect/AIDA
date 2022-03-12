import { Component, OnInit,Input } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import {ActivatedRoute,Router} from '@angular/router';
import {ServiceRegistraionDocument} from '../../../shared/models/service-data';
import {ResourceClientService } from '../../../service/resourceclient.service';
import { MatSnackBar} from '@angular/material/snack-bar';
import { ResponseModel,DropDownModel,DialogDataModel } from '../../../shared/models/common-data';

@Component({
  selector: 'app-enquiry-summary',
  template: `
    <div class="mat-elevation-z4 app-page">

<app-page-toolbar [title]="pageTitle" [addNew]="false" (OnExport)="OnExport($event)"></app-page-toolbar>

<table class="table" mat-table [dataSource]="dataSource" *ngIf="dataSource.data.length>0">
  <ng-container matColumnDef="name">
    <th mat-header-cell *matHeaderCellDef>Name </th>
    <td mat-cell *matCellDef="let element"> {{element.name}} </td>
</ng-container>
<!-- Code Column -->
<ng-container matColumnDef="email">
  <th mat-header-cell *matHeaderCellDef> Email </th>
  <td mat-cell *matCellDef="let element"> {{element.email}} </td>
</ng-container>
<!-- Name Column -->
<ng-container matColumnDef="mobile">
  <th mat-header-cell *matHeaderCellDef> Mobile </th>
  <td mat-cell *matCellDef="let element"> {{element.mobile}} </td>
</ng-container>
  <!-- Remarks Column -->
  <ng-container matColumnDef="documentNames">
    <th mat-header-cell *matHeaderCellDef> Documents </th>
    <td mat-cell *matCellDef="let element"> {{element.documentNames}} </td>
  </ng-container>
  <ng-container matColumnDef="status">
    <th mat-header-cell *matHeaderCellDef> Status </th>
    <td mat-cell *matCellDef="let element"> {{element.status}} </td>
  </ng-container>

  <ng-container matColumnDef="toolbox">
    <th mat-header-cell *matHeaderCellDef width ="40"> </th>
    <td mat-cell *matCellDef="let element"> 
    <mat-button-toggle-group #group="matButtonToggleGroup">

      <mat-button-toggle value="center"  matTooltip="Registraion Completed" 
        matTooltipClass="snackBarBackgroundColor"(click)="OnEdit('Completed',element)">
          <fa-icon [icon]="['fas', 'thumbs-up']"></fa-icon>
      </mat-button-toggle>
      <mat-button-toggle value="right" matTooltip="Registration Enquiry Terminated" 
      matTooltipClass="snackBarBackgroundColor"(click)="OnEdit('Terminated',element)">
          <fa-icon [icon]="['far', 'trash-alt']"></fa-icon>
      </mat-button-toggle>
    </mat-button-toggle-group>
      
    </td>
  </ng-container>

    <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
    <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
  </table>

  <mat-paginator [pageSizeOptions]="[10, 20, 30]" showFirstLastButtons *ngIf="dataSource.data.length>10"></mat-paginator>
  
</div>
  `,
  styles: []
})
export class EnquirySummaryComponent implements OnInit {

  pageTitle = '';

  displayedColumns: string[] = ['name','email','mobile','documentNames','status','toolbox'];

  dataSource = new MatTableDataSource<ServiceRegistraionDocument>();

  constructor(private resourceClient: ResourceClientService,private route: ActivatedRoute,private snackBar: MatSnackBar) { }

  ngOnInit(): void {

    this.route.params.subscribe(params => {
      this.pageTitle = params['displayName'];
      this.resourceClient.getDataInGet('api/service-execution-initial-document/'+params['periods']+'/'+params['status'])
        .subscribe((data: ServiceRegistraionDocument[])=>{
          this.dataSource = new MatTableDataSource<ServiceRegistraionDocument>(data);
      });

    });
  }
  OnExport(paramData)
  {

  }
  OnEdit(option,paramData:ServiceRegistraionDocument) 
  {
    if (paramData.status==='In-Progress')
    {
      paramData.status = option;
      this.OnShowMessage('Please wait updating status...')
      this.resourceClient.getDataInPut('api/service-execution-initial-document',paramData).subscribe((data: ResponseModel)=>{
        this.OnShowMessage(data.message);
      })  
    }
    else
    {
      this.OnShowMessage('This enquiry is already ' + paramData.status);
    }
  } 

  OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }
}
