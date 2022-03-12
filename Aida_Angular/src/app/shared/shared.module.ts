import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule} from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AngularMaterialModule} from './angular-material.module';
import { FontIconModule } from './fonticon.module';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { MatTableExporterModule } from 'mat-table-exporter';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { AngularEditorModule } from '@kolkov/angular-editor';

import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { PageToolbarComponent } from './components/page-toolbar/page-toolbar.component';

import { InputComponent } from "./components/input/input.component";
import { TextAreaComponent } from "./components/textarea/textarea.component";
import { ButtonComponent } from "./components/button/button.component";
import { SelectComponent } from "./components/select/select.component";
import { DateComponent } from "./components/date/date.component";
import { RadiobuttonComponent } from "./components/radiobutton/radiobutton.component";
import { CheckboxComponent } from "./components/checkbox/checkbox.component";
import { DynamicFieldDirective } from "./components/dynamic-field/dynamic-field.directive";
import { DynamicFormComponent } from "./components/dynamic-form/dynamic-form.component";
import { PreviewComponent } from './components/preview/preview.component';
import { CheckboxStripComponent } from './components/checkbox-strip/checkbox-strip.component';
import { ClientPickerComponent } from './components/client-picker/client-picker.component';
import { WebframeComponent } from './components/webframe/webframe.component';
import { CardTableComponent } from './components/card-table/card-table.component';
import { ReportFilterComponent } from './components/report-filter/report-filter.component';
import { SendFormComponent } from './components/send-form/send-form.component';


@NgModule({
  declarations: [PageNotFoundComponent, PageToolbarComponent,InputComponent,ButtonComponent,SelectComponent,DateComponent,
    RadiobuttonComponent,CheckboxComponent,DynamicFieldDirective,DynamicFormComponent, PreviewComponent,
    CheckboxStripComponent,TextAreaComponent, ClientPickerComponent, WebframeComponent, CardTableComponent, 
    ReportFilterComponent, SendFormComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,FlexLayoutModule,
    AngularMaterialModule,
    FontIconModule,
    PdfViewerModule,
    MatTableExporterModule,
    NgxChartsModule,
    AngularEditorModule,
    HttpClientModule
  ],
  exports:[DynamicFieldDirective,DynamicFormComponent,PageToolbarComponent,PreviewComponent,CheckboxStripComponent,
    ClientPickerComponent,CardTableComponent,MatTableExporterModule,NgxChartsModule,SendFormComponent],
  entryComponents: [
    InputComponent,ButtonComponent,SelectComponent,DateComponent,RadiobuttonComponent,CheckboxComponent,
    PreviewComponent,CheckboxStripComponent,TextAreaComponent,SendFormComponent
  ]
})
export class SharedModule { }
