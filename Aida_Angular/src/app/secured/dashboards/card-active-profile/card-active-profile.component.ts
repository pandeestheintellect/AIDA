import { Component, OnInit,Input } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import { Router} from '@angular/router';

import {StatusCountModel} from '../../../shared/models/dashboard-data';
import {ResourceClientService } from '../../../service/resourceclient.service';

@Component({
  selector: 'app-card-active-profile',
  template: `
    
    <table class="table mat-elevation-z4" mat-table [dataSource]="dataSource">
    
        <ng-container matColumnDef="name">
          <th mat-header-cell *matHeaderCellDef> Name </th>
          <td mat-cell *matCellDef="let element"> {{element.name}} </td>
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
export class CardActiveProfileComponent implements OnInit {

  displayedColumns: string[] = ['name','counts','toolbox'];

  dataSource = new MatTableDataSource<any>();


  constructor(private resourceClient: ResourceClientService,private router: Router) { }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/dashboard-active-profile').subscribe((data: any[])=>{
      this.dataSource = new MatTableDataSource<any>(data);
      
    })  
    
  }

  OnView(data:any) {
    this.router.navigate(['/secured/dashboards/client-view',data.businessProfileId]);
  }
}
