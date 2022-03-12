import { Component, OnInit,Input } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import { Router} from '@angular/router';

import {StatusCountModel} from '../../../shared/models/dashboard-data';
import {ResourceClientService } from '../../../service/resourceclient.service';

@Component({
  selector: 'app-card-client-summary',
  template: `
        <table class="table mat-elevation-z4" mat-table [dataSource]="dataSource">
        <ng-container matColumnDef="displayName">
          <th mat-header-cell *matHeaderCellDef> Periods </th>
          <td mat-cell *matCellDef="let element"> {{element.displayName}} </td>
        </ng-container>
    
        <ng-container matColumnDef="counts">
          <th mat-header-cell *matHeaderCellDef> Count </th>
          <td mat-cell *matCellDef="let element"> {{element.counts}} </td>
        </ng-container>

        <ng-container matColumnDef="toolbox">
          <th mat-header-cell *matHeaderCellDef width ="40"> </th>
          <td mat-cell *matCellDef="let element"> 
              <mat-button-toggle-group #group="matButtonToggleGroup">
                  <mat-button-toggle value="center" matTooltip=" View {{element.displayName}} of {{element.status}} summary."   
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
export class CardClientSummaryComponent implements OnInit {

  @Input()  status: string;

  displayedColumns: string[] = ['displayName','counts','toolbox'];

  dataSource = new MatTableDataSource<StatusCountModel>();


  constructor(private resourceClient: ResourceClientService,private router: Router) { }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/dashboard-enquiry/'+this.status).subscribe((data: StatusCountModel[])=>{
      this.dataSource = new MatTableDataSource<StatusCountModel>(data);
      
    })  
    
  }

  OnView(data:StatusCountModel) {
    this.router.navigate(['secured/reports/enquiry-summary',data.periods,data.status,data.displayName]);
  }
}
