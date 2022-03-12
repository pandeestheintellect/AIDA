import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';

import { ResourceClientService } from '../../../service/resourceclient.service';
import { DropDownModel } from '../../../shared/models/common-data';

@Component({
  selector: 'app-client-picker',
  template: `
    <div  fxLayout="row" fxLayoutWrap class="control-bar">

          <mat-form-field>
            <mat-label>Choose Client Profile</mat-label>
            <mat-select [(ngModel)]="clientProfileId" name="clientProfileName">
                <mat-option *ngFor="let type of companyNameList" [value]="type.value">{{type.text}}</mat-option>
            </mat-select>
          </mat-form-field>
        
          <div style="padding: 10px;height: 40px;">
              <button mat-button mat-flat-button  (click)="Get()" color="primary">Get</button>
          </div>
  
      </div>
  `,
  styles: []
})
export class ClientPickerComponent implements OnInit {

  @Input()  clientProfileId: number;
  @Output() OnGet = new EventEmitter<number>();
  
   
  companyNameList: DropDownModel[] = [];

  constructor(private resourceClient: ResourceClientService) { }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/masters/dropdown/business-profile').subscribe((data: DropDownModel[])=>{
      this.companyNameList = data;
    })
  }

  Get()
  {
    this.OnGet.emit(this.clientProfileId);
  }
}
