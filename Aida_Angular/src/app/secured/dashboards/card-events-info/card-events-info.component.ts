import { Component, OnInit,Input } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import { Router} from '@angular/router';

import {StatusCountModel} from '../../../shared/models/dashboard-data';
import {ResourceClientService } from '../../../service/resourceclient.service';
import { MatTabChangeEvent } from '@angular/material/tabs';

import * as _moment from 'moment';

const moment = _moment;

@Component({
  selector: 'app-card-events-info',
  template: ` 
    
    <mat-tab-group #tabGroup (selectedTabChange)="OnTabChanged($event)">
      <mat-tab label="Events happening today"></mat-tab>
      <mat-tab label="Events after 5th day"></mat-tab>
      <mat-tab label="Events after 10th day"></mat-tab>
    </mat-tab-group>   

    <table class="table mat-elevation-z4" mat-table [dataSource]="dataSource">
    <ng-container matColumnDef="eventName">
      <th mat-header-cell *matHeaderCellDef> Event </th>
      <td mat-cell *matCellDef="let element"> {{element.eventName}} </td>
    </ng-container>    
    <ng-container matColumnDef="name">
      <th mat-header-cell *matHeaderCellDef> Name </th>
      <td mat-cell *matCellDef="let element"> {{element.name}} </td>
    </ng-container>
    <ng-container matColumnDef="entity">
      <th mat-header-cell *matHeaderCellDef> Entity </th>
      <td mat-cell *matCellDef="let element"> {{element.entity}} </td>
    </ng-container>
    <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
    <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
  </table> 


  `,
  styles: []
})
export class CardEventsInfoComponent implements OnInit {

  

  displayedColumns: string[] = ['eventName','name','entity'];

  dataSource = new MatTableDataSource<any>();


  constructor(private resourceClient: ResourceClientService,private router: Router) { }

  ngOnInit(): void {
    var eventDate = moment(new Date()).format('YYYY-MM-DD');
    this.OnLoad(eventDate);
    
  } 
  OnLoad(eventDate:string)
  {
    this.resourceClient.getDataInGet('api/dashboard-events-info/'+eventDate).subscribe((data: any[])=>{
      this.dataSource = new MatTableDataSource<any>(data);
      
    })  
  }
  OnTabChanged(tabChangeEvent: MatTabChangeEvent) {
  
    var eventDate = moment(new Date());
    if (tabChangeEvent.index===1)
      eventDate = eventDate.add(5,'days');
    else if (tabChangeEvent.index===2)
      eventDate = eventDate.add(10,'days');  
    
    this.OnLoad(eventDate.format('YYYY-MM-DD'));
  }
  OnView(data:any) {
    this.router.navigate(['/secured/dashboards/client-view',data.businessProfileId]);
  }
}
