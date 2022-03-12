import { Component, OnInit,Input } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import { Router} from '@angular/router';

import {ServiceStatusCountModel} from '../../../shared/models/dashboard-data';
import {ResourceClientService } from '../../../service/resourceclient.service';


@Component({
  selector: 'app-card-services-status',
  template: `

    <table class="table mat-elevation-z4" mat-table [dataSource]="dataSource">
        <ng-container matColumnDef="name">
          <th mat-header-cell *matHeaderCellDef> Service </th>
          <td mat-cell *matCellDef="let element"> {{element.name}} </td>
        </ng-container>
    
        <ng-container matColumnDef="monthlyCounts">
          <th mat-header-cell *matHeaderCellDef> Month </th>
          <td mat-cell *matCellDef="let element">  
            <mat-button-toggle-group #group="matButtonToggleGroup">
                <mat-button-toggle value="center" matTooltip="View monthly {{element.name}} services {{status}} count."   
                    matTooltipClass="snackBarBackgroundColor" (click)="OnView('monthly',element)">
                    {{element.monthlyCounts}}
                </mat-button-toggle>
              </mat-button-toggle-group>
          </td>
        </ng-container>
        <ng-container matColumnDef="yearlyCounts">
          <th mat-header-cell *matHeaderCellDef> Year </th>
          <td mat-cell *matCellDef="let element"> 
          <mat-button-toggle-group #group="matButtonToggleGroup">
                <mat-button-toggle value="center" matTooltip="View yearly {{element.name}} services {{status}} count."   
                    matTooltipClass="snackBarBackgroundColor" (click)="OnView('yearly',element)">
                    {{element.yearlyCounts}}
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
export class CardServicesStatusComponent implements OnInit {

  @Input()  status: string;

  cardType=1;

  displayedColumns: string[] = ['name','monthlyCounts','yearlyCounts'];

  dataSource = new MatTableDataSource<ServiceStatusCountModel>();


  constructor(private resourceClient: ResourceClientService,private router: Router) { }

  ngOnInit(): void {
    
    this.OnLoad();
  } 
  OnLoad()
  {
    this.resourceClient.getDataInGet('api/dashboard-services-status/'+this.status).subscribe((data: ServiceStatusCountModel[])=>{
      this.dataSource = new MatTableDataSource<ServiceStatusCountModel>(data);
      
    })  
  }

  OnView(period:string, data:ServiceStatusCountModel) {
    this.router.navigate(['/secured/services/clients', data.serviceCode.toLowerCase(),this.status.toLowerCase(),period]);
  }
  OnSwitch(viewId:number)
  {
    this.cardType=viewId;
  }
}
