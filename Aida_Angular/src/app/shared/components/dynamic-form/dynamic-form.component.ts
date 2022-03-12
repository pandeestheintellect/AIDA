import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output
} from "@angular/core";
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl
} from "@angular/forms";
import { FieldConfig, Validator } from "../field.interface";

import { FormaterService } from '../../../service/formater.service';

import * as _moment from 'moment';

const moment = _moment;

@Component({
  exportAs: "dynamicForm",
  selector: "dynamic-form",
  template: `
  <form [formGroup]="form" (submit)="onSubmit($event)" class="SOP-form" disabled> 
    <div fxLayout="row wrap"  fxLayoutGap="1%" fxLayoutAlign="left center" >
      <div *ngFor="let field of fields;" fxFlex="{{field.fxSizePer}}" fxFlex.gt-sm="{{field.fxSizePer}}"  style="padding:4px !important;">
        <div fxFlex="100%" fxFlex.gt-sm="100%" *ngIf="field.type!='label'" > <ng-container dynamicField [field]="field" [group]="form" ></ng-container></div>
        <div fxFlex="100%" fxFlex.gt-sm="100%" *ngIf="field.type=='label'" style="margin-bottom:10px;font-Size:16px;text-align: justify;">{{field.label}}</div>  
      </div>
    </div>
  </form>
  `,
  styles: []
})
export class DynamicFormComponent implements OnInit {

  /*
  <form [formGroup]="form" (submit)="onSubmit($event)" class="SOP-form"> 
    <div fxLayout="row wrap"  fxLayoutGap="20px" fxLayoutAlign="left center">
      <div *ngFor="let field of fields;" fxFlex="auto" fxFlex.gt-sm="{{field.fxSizePer}}" style="padding:10px !important;" >
        <div fxFlex="auto" fxFlex.gt-sm="3%" style="margin-right: 16px; font-size:18px;"> {{field.controlNumber}}</div>
        <div fxFlex="auto" fxFlex.gt-sm="97%" *ngIf="field.type!='label'"> <ng-container dynamicField [field]="field" [group]="form" ></ng-container></div>
        <div fxFlex="auto" fxFlex.gt-sm="97%" *ngIf="field.type=='label'" style="margin-bottom:10px;font-Size:16px;text-align: justify;">{{field.label}}</div>  
      </div>
    </div>
  </form>
  */
  @Input() fields: FieldConfig[] = [];

  @Output() submit: EventEmitter<any> = new EventEmitter<any>();

  
  form: FormGroup;

  get value() {
    return this.form.value;
  }
  constructor(private fb: FormBuilder,public formater:FormaterService) {}

  ngOnInit() {
    this.form = this.createControl();
  }
  onDisable()
  {
    this.form.disable();
  }
  onSubmit(event: Event) {
    event.preventDefault();
    event.stopPropagation();
    if (this.form.valid) {

      Object.keys(this.form.controls).forEach(field => {
        if (field.startsWith("D:") || field.startsWith("DM:"))
        {
          const control = this.form.get(field);
          var aDate   = moment(control.value, 'YYYY-MM-DD', true);

          if (aDate.isValid())
            control.setValue(aDate.format('YYYY-MM-DD')) ;
          else
            control.setValue(null) ; 
            
        }
        
      });

      this.submit.emit(this.form.value);
    } else {
      this.validateAllFormFields(this.form);
    }
  }

  createControl() {
    const group = this.fb.group({});
    this.fields.forEach(field => {
      if (field.type === "button") return;
      
      if (field.type==='date')
        field.value = moment(field.value,'DD/MM/YYYY');
      
      if (field.inputType!==null && field.inputType==='currency')
      {
        field.inputType='text';
        field.type='input';
        field.format= 'currency';
        field.value =this.formater.getFormatedCurrencyValue(field.value);
      }
        

      const control = this.fb.control(
        field.value,
        this.bindValidations(field.validations || [])
      );
      group.addControl(field.name, control);
    });
    return group;
  }

  bindValidations(validations: any) {
    if (validations.length > 0) {
      const validList = [];
      validations.forEach(valid => {
        validList.push(valid.validator);
      });
      return Validators.compose(validList);
    }
    return null;
  }

  validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      control.markAsTouched({ onlySelf: true });
    });
  }
}
