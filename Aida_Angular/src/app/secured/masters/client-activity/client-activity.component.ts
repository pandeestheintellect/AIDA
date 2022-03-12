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
  selector: 'app-business-activity',
  templateUrl: './client-activity.component.html',
  styles: []
})
export class ClientActivityComponent implements OnInit {

  businessProfileId: number;
  selectedIndustry:string='';
  otherName:string='';
  otherDescription:string='';
  activityListControl = new FormControl();
  industryClassification: DropDownModel[] =[];
  industryClassificationSelected: ClientProfileActivity[] =[];
  filteredOptions: Observable<DropDownModel[]>;

  displayedColumns: string[] = ['name','description','toolbox'];
  
  dataSource = new MatTableDataSource<ClientProfileActivity>(this.industryClassificationSelected);

   
  constructor(public dialogRef: MatDialogRef<ClientActivityComponent>,
    @Inject(MAT_DIALOG_DATA) public data: number,private resourceClient: ResourceClientService) {
      this.businessProfileId = data;
     }

    ngOnInit(): void {
      this.resourceClient.getDataInGet('api/masters/dropdown/acra-industry-classification').subscribe((data: DropDownModel[])=>{
        
        this.industryClassification = data;

        this.filteredOptions = this.activityListControl.valueChanges
          .pipe(
            startWith(''),
            map(value => this.OnFilter(value))
          );
          
      })  

      this.resourceClient.getDataInGet('api/business-activity/'+this.businessProfileId).subscribe((data: ClientProfileActivity[])=>{
        
        this.dataSource = new MatTableDataSource<ClientProfileActivity>(data);
      })  
      
    }
    onSelectionChanged(event) {
      this.selectedIndustry = event.option.viewValue;
    }
    OnAdd()
    {
      var selected:string[] = this.selectedIndustry.split(':');
      this.AddToSelected(selected[0],selected[1]);
    }
    OnAddOthers()
    {
      this.AddToSelected(this.otherName,this.otherDescription);
    }
    AddToSelected(code:string,name:string)
    {
      for(var data of this.industryClassificationSelected)
      {
        if (code ===data.name && name ===data.description)
          return;
      }

      this.industryClassificationSelected.push({businessProfileId: this.businessProfileId, name:code,description:name})
      this.dataSource = new MatTableDataSource<ClientProfileActivity>(this.industryClassificationSelected);
    }
    OnDelete(industryData){

      var industry: ClientProfileActivity[] =[];

      for(var data of this.industryClassificationSelected)
      {
        if (industryData.value !==data.name || industryData.text !==data.description)
          industry.push(data)
      }

      this.industryClassificationSelected = industry;
      this.dataSource = new MatTableDataSource<ClientProfileActivity>(this.industryClassificationSelected);
    }
    OnOK(){
      if (this.industryClassificationSelected.length===0)
      {
        this.industryClassificationSelected.push({businessProfileId:this.businessProfileId,name:'DELETE',description:'DELETE'})
      }
      this.dialogRef.close({event:'Update',data:this.industryClassificationSelected});
    }
  
    OnClose(){
      this.dialogRef.close({event:'Cancel'});
    }

    OnFilter(value: string): DropDownModel[] {
      const filterValue = value.toLowerCase();
  
      return this.industryClassification.filter(option => 
          (option.value.toLowerCase() + option.text.toLowerCase()) .includes(filterValue));
    }
}
