import { Component, OnInit,Input } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import { Router} from '@angular/router';

import {StatusCountModel} from '../../../shared/models/dashboard-data';
import {ResourceClientService } from '../../../service/resourceclient.service';

@Component({
  selector: 'app-card-services-summary',
  template: `
    
    <table class="table mat-elevation-z4" mat-table [dataSource]="dataSource">
        <ng-container matColumnDef="created">
          <th mat-header-cell *matHeaderCellDef> Month </th>
          <td mat-cell *matCellDef="let element"> {{element.created}} </td>
        </ng-container>
    
        <ng-container matColumnDef="name">
          <th mat-header-cell *matHeaderCellDef> Service </th>
          <td mat-cell *matCellDef="let element"> {{element.name}} </td>
        </ng-container>
        <ng-container matColumnDef="clientType">
          <th mat-header-cell *matHeaderCellDef> Entity Type </th>
          <td mat-cell *matCellDef="let element"> {{element.clientType}} </td>
        </ng-container>
        <ng-container matColumnDef="status">
          <th mat-header-cell *matHeaderCellDef> Status </th>
          <td mat-cell *matCellDef="let element"> {{element.status}} </td>
        </ng-container>    
        <ng-container matColumnDef="counts">
          <th mat-header-cell *matHeaderCellDef> Count </th>
          <td mat-cell *matCellDef="let element"> {{element.counts}} </td>
        </ng-container>
        <ng-container matColumnDef="toolbox">
          <th mat-header-cell *matHeaderCellDef width ="40"> </th>
          <td mat-cell *matCellDef="let element"> 
              <mat-button-toggle-group #group="matButtonToggleGroup">
                  <mat-button-toggle value="center" matTooltip=" View {{element.created}} {{element.name}} service {{element.status}} status."   
                      matTooltipClass="snackBarBackgroundColor" (click)="OnView(element)">
                    <fa-icon [icon]="['fas', 'arrow-right']"></fa-icon>
                  </mat-button-toggle>
                </mat-button-toggle-group>
          </td>
        </ng-container>
  
        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
      </table> 

  `,
  styles: []
})
export class CardServicesSummaryComponent implements OnInit {

  displayedColumns: string[] = ['created','name','clientType','status','counts','toolbox'];

  dataSource = new MatTableDataSource<any>();


  constructor(private resourceClient: ResourceClientService,private router: Router) { }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/dashboard-services-entity-summary').subscribe((data: any[])=>{
      this.dataSource = new MatTableDataSource<any>(data);
      
    })  
    
  }

  OnView(data:any) {
    this.router.navigate(['/secured/services/clients',data.name.toLowerCase(),data.status.toLowerCase(),data.createdMonth,data.clientType.toLowerCase()]);
  }
}
