import { Component, EventEmitter, Input, Output } from '@angular/core';


@Component({
  selector: 'app-checkbox-strip', 
  template: ` 
    <div fxLayout="row wrap"  fxLayoutGap="20px" fxLayoutAlign="left center">
      <div *ngFor="let box of checkboxList;" fxFlex="auto"  style="padding:10px !important;" >
        <div fxFlex="auto" fxFlex.gt-sm="3%" style="margin-right: 16px; font-size:18px;"> 
          <mat-checkbox  color="primary">{{box.text}}</mat-checkbox>
        </div>
      </div>
    </div>
  `,
  styles: []
})
export class CheckboxStripComponent {

  @Input()  title: string;
  @Input()  addNew: boolean;
  @Output() OnAdd = new EventEmitter<void>();
  @Output() OnExport = new EventEmitter<number>();

  checkboxList = [{text:"One 1"},{text:"One 2"},{text:"One 3"},{text:"One 4"}];

  Add() {
    this.OnAdd.emit();
  }

  Export(fileType: number) {
    this.OnExport.emit(fileType);
  }

}
