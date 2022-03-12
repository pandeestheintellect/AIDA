import { Component, OnInit } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { FieldConfig } from "../field.interface";

import { FormaterService } from '../../../service/formater.service';

@Component({
  selector: "app-input", 
  template: `
                        
  <mat-form-field fxFlex [formGroup]="group">
    <input matInput [formControlName]="field.name" [placeholder]="field.label" [type]="field.inputType" (blur)="transformAmount($event)">
    <ng-container *ngFor="let validation of field.validations;" ngProjectAs="mat-error">
      <mat-error *ngIf="group.get(field.name).hasError(validation.name)">{{validation.message}}</mat-error>
    </ng-container>
  </mat-form-field>

`,
  styles: []
})
export class InputComponent implements OnInit {
  field: FieldConfig;
  group: FormGroup;
  constructor(public formater:FormaterService) {}
  ngOnInit() {}

  
  transformAmount(element)
  {
    if (this.field.format==='currency')
    {
      element.target.value= this.formater.getFormatedCurrencyValue(element.target.value);
    }
  }
}
