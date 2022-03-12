import { Component, EventEmitter, Input, Output } from '@angular/core';


@Component({
  selector: 'app-page-toolbar', 
  template: ` 
    <mat-toolbar class="page-toolbar">
        <h2 class="mat-headline center-align" style="margin-left: 20px;">{{title}}</h2>
        <span class="header-spacer"></span>
        <mat-form-field fxFlex="20%" style=" font-size: 14px !important;padding-right:10px" *ngIf="hasFilter == true">
          <input matInput type="text" (keyup)="Filter($event.target.value)" placeholder={{filterCaption}}  [(ngModel)]="filterText">
          <button mat-button *ngIf="filterText" matSuffix mat-icon-button aria-label="Clear" (click)="onFilterCler()">
          <mat-icon>close</mat-icon>
        </button>
        </mat-form-field>
        <mat-button-toggle-group #group="matButtonToggleGroup">
          <mat-button-toggle value="center" color="primary" (click)="Add()" matTooltip={{addToolTip}}  
                matTooltipClass="snackBarBackgroundColor" *ngIf="addNew != false">
            <fa-icon [icon]="toolbarIconPlus"></fa-icon>
          </mat-button-toggle>
          <mat-button-toggle value="center"  color="primary" (click)="Custom()" matTooltip={{customToolTip}} 
                matTooltipClass="snackBarBackgroundColor" *ngIf="hasCustomIcon == true">
            <fa-icon [icon]="customIcon"></fa-icon>
          </mat-button-toggle>
          <mat-button-toggle value="center"  (click)="Export(1)" matTooltip="Export to Excel"  
                matTooltipClass="snackBarBackgroundColor" > 
            <fa-icon [icon]="toolbarIconExcel"></fa-icon>
          </mat-button-toggle>
          <!--
          <mat-button-toggle value="right"  (click)="Export(2)" matTooltip="Export to PDF" 
                matTooltipClass="snackBarBackgroundColor">
            <fa-icon [icon]="toolbarIconPDF"></fa-icon>
          </mat-button-toggle>
-->
        </mat-button-toggle-group>
    </mat-toolbar>
  `,
  styles: []
})
export class PageToolbarComponent {

  @Input()  title: string;
  @Input()  addNew: boolean;
  @Input()  addToolTip: string;

  @Input()  customIcon: any;
  @Input()  hasCustomIcon: boolean;
  @Input()  customToolTip: string;
  
  @Input()  hasFilter: boolean;
  @Input()  filterCaption: string;

  @Output() OnAdd = new EventEmitter<void>();
  @Output() OnExport = new EventEmitter<number>();
  @Output() OnCustom = new EventEmitter<void>();
  @Output() OnFilter = new EventEmitter<string>();
  
  toolbarIconPlus={ iconName: 'plus-square', prefix: 'far' };
  toolbarIconExcel={ iconName: 'file-excel', prefix: 'far' };
  toolbarIconPDF={ iconName: 'file-pdf', prefix: 'far' };

  filterText = '';

  //toolbarIconCustom={ iconName: 'file-pdf', prefix: 'fas' };

  Add() {
    this.OnAdd.emit();
  }
  
  Custom() {
    this.OnCustom.emit();
  }

  Export(fileType: number) {
    this.OnExport.emit(fileType);
  }
  onFilterCler(){
    this.filterText='';
    this.Filter('')  ;
  }
  Filter(filterText:string) {
   this.OnFilter.emit(filterText); 
  }
}
