import {Component, OnInit, ViewChild, EventEmitter, Input, Output} from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';

import {TableData} from './card-table.data';



@Component({
  selector: 'app-card-table',
  template: `

<mat-table #table [dataSource]="dataSource" class="table mat-elevation-z4">

  <ng-container *ngFor="let column of columns" [cdkColumnDef]="column.columnDef">
    <mat-header-cell *cdkHeaderCellDef>{{ column.header }}</mat-header-cell>
    <mat-cell *cdkCellDef="let row">{{ column.cell(row) }}</mat-cell>
  </ng-container>
  <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
  <mat-row *matRowDef="let row; columns: displayedColumns;"></mat-row>
</mat-table>

<mat-paginator [pageSizeOptions]="[20,30 ]" showFirstLastButtons *ngIf="noOfRows>maxDisplayRows"></mat-paginator>

  `,
  styles: []
})
export class CardTableComponent implements OnInit {

  @ViewChild(MatPaginator) paginator: MatPaginator;

  @Input()  tableData: TableData;
  
  displayedColumns:any;
  columns:any;
  dataSource:any;
  noOfRows:number = 0;
  maxDisplayRows:number = 0;

  constructor() { }

  ngOnInit(): void { 
  
    
    this.displayedColumns = this.tableData.getDisplayedColumns();
    this.columns = this.tableData.getColumns();
    this.dataSource = this.tableData.getDataSource();
    this.noOfRows = this.tableData.getNoOfRows();
    this.maxDisplayRows = this.tableData.getNoOfRows();
  
  }

}
