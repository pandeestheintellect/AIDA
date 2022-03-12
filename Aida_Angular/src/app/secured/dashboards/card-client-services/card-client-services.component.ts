import { Component, OnInit, ViewChild,Input } from '@angular/core';
import { HttpUrlEncodingCodec } from '@angular/common/http';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table'; 
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';

import { ResponseModel,DropDownModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { ServicesDefinitionModel } from '../../../shared/models/service-data';
import { ServiceRegistrationViewModel } from '../../../shared/models/service-data';


@Component({
  selector: 'app-card-client-services',
  template: `
    <h2>Services Activity (last 6 services)</h2> 
    <table class="table mat-elevation-z4" mat-table [dataSource]="dataSource" *ngIf="dataSource.data.length>0">
       
       <ng-container matColumnDef="serviceName">
         <th mat-header-cell *matHeaderCellDef> Service </th>
         <td mat-cell *matCellDef="let element"> {{element.serviceName}} </td>
       </ng-container>
     <!-- Name Column -->
     <ng-container matColumnDef="officerName">
         <th mat-header-cell *matHeaderCellDef> Officer Name </th>
         <td mat-cell *matCellDef="let element"> {{element.officerName}} </td>
     </ng-container>
         <!-- Remarks Column -->
         <ng-container matColumnDef="created">
           <th mat-header-cell *matHeaderCellDef> Date </th>
           <td mat-cell *matCellDef="let element"> {{element.created}} </td>
         </ng-container>

         <ng-container matColumnDef="status">
           <th mat-header-cell *matHeaderCellDef> Status </th>
           <td mat-cell *matCellDef="let element"> {{element.status}} </td>
         </ng-container>
       <!-- Get Details -->
     <ng-container matColumnDef="toolbox">
         <th mat-header-cell *matHeaderCellDef width ="40"> </th>
         <td mat-cell *matCellDef="let element"> 
             <mat-button-toggle-group #group="matButtonToggleGroup" >
               <mat-button-toggle value="center" matTooltip="Start filling the details for {{element.officerName}}" 
                 matTooltipClass="snackBarBackgroundColor" (click)="OnView(element)">
                 <fa-icon [icon]="['fas', 'play']"></fa-icon>
               </mat-button-toggle>
                 
               </mat-button-toggle-group>
         </td>
       </ng-container>
 
       <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
       <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
     </table>
   
     <mat-paginator [pageSizeOptions]="[10, 20, 30]" showFirstLastButtons *ngIf="dataSource.data.length>10"></mat-paginator>

  `,
  styles: []
})
export class CardClientServicesComponent implements OnInit {

  @Input()  clientProfileId: string;

  displayedColumns: string[] = ['serviceName','officerName','created','status','toolbox'];
  
  dataSource = new MatTableDataSource<ServiceRegistrationViewModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  constructor(private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router) { }

  ngOnInit(): void {
    if (this.clientProfileId!=='0')
        this.OnLoad(this.clientProfileId);
  }

  OnLoad(clientProfileId:string)
  {
    
      this.resourceClient.getDataInGet('api/service-registration-view/'+clientProfileId).subscribe((data: ServiceRegistrationViewModel[])=>{
       
        this.dataSource = new MatTableDataSource<ServiceRegistrationViewModel>(data);
        this.dataSource.paginator = this.paginator;
      })  
    
  }
  OnView(paramData:ServiceRegistrationViewModel)
  {
    this.router.navigate(['secured/services/listing/'+ paramData.serviceCode +'/'+ 
      paramData.businessProfileId+'/'+paramData.status+'/'+paramData.serviceBusinessId]); 
  }

}
