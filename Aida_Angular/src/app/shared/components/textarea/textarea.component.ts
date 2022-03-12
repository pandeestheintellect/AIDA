import { Component, OnInit } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { FieldConfig } from "../field.interface";

import { FormaterService } from '../../../service/formater.service';

@Component({
  selector: "app-textarea", 
  template: `
                        
  <mat-form-field fxFlex [formGroup]="group">
    <mat-label>{{field.label}}</mat-label>
    <textarea matInput [placeholder]="field.label" [formControlName]="field.name"
            cdkTextareaAutosize
            #autosize="cdkTextareaAutosize"
            cdkAutosizeMinRows="1"
            cdkAutosizeMaxRows="5"></textarea>
  </mat-form-field>

`,
  styles: []
})
export class TextAreaComponent implements OnInit {
  field: FieldConfig;
  group: FormGroup;
  constructor() {}
  ngOnInit() {}
}
