import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import {FormControl} from '@angular/forms';
import { ResourceClientService } from '../../../service/resourceclient.service';
import {Observable} from 'rxjs';
import {map, startWith} from 'rxjs/operators';
import { DropDownModel,DialogDataModel } from '../../../shared/models/common-data'
import {MatTableDataSource} from '@angular/material/table';
import { ClientProfileModel,ClientProfileActivity } from '../../../shared/models/client-profile-data';


@Component({
  selector: 'app-service-documents',
  templateUrl: './service-documents.component.html',
  styles: []
})
export class ServiceDocumentsComponent implements OnInit {

  serviceDetails:DropDownModel;
  selectedDocument:string='';
  documentsListControl = new FormControl();
  documents: DropDownModel[] =[];
  documentsSelected: DropDownModel[] =[];
  filteredOptions: Observable<DropDownModel[]>;

  displayedColumns: string[] = ['value','text','toolbox'];
  
  dataSource = new MatTableDataSource<DropDownModel>(this.documentsSelected);


  constructor(public dialogRef: MatDialogRef<ServiceDocumentsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DropDownModel,private resourceClient: ResourceClientService) { 
      this.serviceDetails=data;
    }

  ngOnInit(): void {

    this.resourceClient.getDataInGet('api/service-definitions-documents/'+this.serviceDetails.value).subscribe((data: DropDownModel[])=>{
        
      this.dataSource = new MatTableDataSource<DropDownModel>(data);
    
      this.documentsSelected = data;
    
        
    })  

    this.resourceClient.getDataInGet('api/masters/dropdown/documents').subscribe((data: DropDownModel[])=>{
        
      this.documents = data;

      this.filteredOptions = this.documentsListControl.valueChanges
        .pipe(
          startWith(''),
          map(value => this.OnFilter(value))
        );
        
    })  

  }
  OnFilter(value: string): DropDownModel[] {
    const filterValue = value.toLowerCase();

    return this.documents.filter(option => 
        (option.value.toLowerCase() + option.text.toLowerCase()) .includes(filterValue));
  }
  onSelectionChanged(event) {
    this.selectedDocument = event.option.viewValue;
  }
  OnAdd()
  {
    var selected:string[] = this.selectedDocument.split(':');
    this.AddToSelected(selected[0],selected[1]);
  }
  AddToSelected(code:string,name:string)
  {
    
    for(var data of this.documentsSelected)
    {
      if (code ===data.value && name ===data.text)
        return;
    }

    this.documentsSelected.push({value:code,text:name})
    this.dataSource = new MatTableDataSource<DropDownModel>(this.documentsSelected);
    
  }
  OnDelete(documentsData:DropDownModel)
  {

    var documents: DropDownModel[] =[];

    for(var data of this.documentsSelected)
    {
      if (documentsData.value ===data.value && documentsData.text ===data.text)
        continue;
      else
        documents.push(data);
    }

    this.documentsSelected = documents;
    this.dataSource = new MatTableDataSource<DropDownModel>(this.documentsSelected);
  }
  OnOK(){
    var postdata:DropDownModel[];
    postdata = this.documentsSelected;
    postdata.push({value:this.serviceDetails.value,text:"#ServiceCode#"});
    this.dialogRef.close({event:'Update',data:postdata});
  }

  OnClose(){
    this.dialogRef.close({event:'Cancel'});
  }
} 
