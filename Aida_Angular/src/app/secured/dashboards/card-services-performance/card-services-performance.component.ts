import { Component, OnInit } from '@angular/core';

import {ResourceClientService } from '../../../service/resourceclient.service';

@Component({
  selector: 'app-card-services-performance',
  template: `

    <div fxFlex="auto" fxFlex.gt-sm="50%">
    <ngx-charts-line-chart
  [view]="view"
  [scheme]="colorScheme"
  [legend]="legend"
  [showXAxisLabel]="showXAxisLabel"
  [showYAxisLabel]="showYAxisLabel"
  [xAxis]="xAxis"
  [yAxis]="yAxis"
  [xAxisLabel]="xAxisLabelNew"
  [yAxisLabel]="yAxisLabel"
  [timeline]="timeline"
  [results]="newServices"

  >
</ngx-charts-line-chart>
         
    </div>

    <div fxFlex="auto" fxFlex.gt-sm="50%">
    <ngx-charts-line-chart
  [view]="view"
  [scheme]="colorScheme"
  [legend]="legend"
  [showXAxisLabel]="showXAxisLabel"
  [showYAxisLabel]="showYAxisLabel"
  [xAxis]="xAxis"
  [yAxis]="yAxis"
  [xAxisLabel]="xAxisLabelCompleted"
  [yAxisLabel]="yAxisLabel"
  [timeline]="timeline"
  [results]="completedServices"

  >
</ngx-charts-line-chart>
         
    </div>

  `,
  styles: []
})
export class CardServicesPerformanceComponent implements OnInit {

  newServices: any[];
  completedServices: any[];

  view: any[] = [550, 300];

  // options
  legend: boolean = true;
  showLabels: boolean = true;
  animations: boolean = true;
  xAxis: boolean = true;
  yAxis: boolean = true;
  showYAxisLabel: boolean = true;
  showXAxisLabel: boolean = true;
  xAxisLabelNew: string = 'Services Created';
  xAxisLabelCompleted: string = 'Services Completed';
  yAxisLabel: string = 'No of Services';
  timeline: boolean = true;

  colorScheme = {
    domain: ['#5AA454', '#E44D25', '#CFC0BB', '#7aa3e5', '#a8385d', '#aae3f5']
  };

  constructor(private resourceClient: ResourceClientService) { }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/dashboard-services-performance/new').subscribe((data: [])=>{
      this.newServices = data;
      
    })  
    this.resourceClient.getDataInGet('api/dashboard-services-performance/completed').subscribe((data: [])=>{
      this.completedServices = data;
      
    })  
  }

}
