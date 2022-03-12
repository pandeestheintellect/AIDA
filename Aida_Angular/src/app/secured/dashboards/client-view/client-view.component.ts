import { Component, OnInit,ViewChild } from '@angular/core';

import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {ActivatedRoute,Router} from '@angular/router';
import { MatSnackBar} from '@angular/material/snack-bar';

import { ResponseModel,DropDownModel } from '../../../shared/models/common-data';
import { ResourceClientService } from '../../../service/resourceclient.service';
import { CardClientProfileComponent } from '../card-client-profile/card-client-profile.component';
import { CardClientServicesComponent } from '../card-client-services/card-client-services.component';
import { UploadComponent } from '../../services/upload/upload.component';

@Component({
  selector: 'app-client-view',
  templateUrl: './client-view.component.html',
  styles: []
})
export class ClientViewComponent implements OnInit {

  @ViewChild('clientProfileSummary', {static: false}) clientProfileSummaryChild:CardClientProfileComponent;
  @ViewChild('clientProfileServices', {static: false}) clientProfileServicesChild:CardClientServicesComponent;

  clientProfileId='0';  
  clientProfileList: DropDownModel[] = [];
  showDetail:boolean=false;

  constructor(private dialog: MatDialog,private resourceClient: ResourceClientService,private route: ActivatedRoute,
    private router: Router,private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.resourceClient.getDataInGet('api/masters/dropdown/business-profile').subscribe((data: DropDownModel[])=>{
      this.clientProfileList = data;
    })
    this.route.params.subscribe(params => {
      this.clientProfileId = params['businessProfileId'];
      if (this.clientProfileId==null || this.clientProfileId==undefined)
        this.clientProfileId='0';

      if (this.clientProfileId!=='0')
      {
        this.OnSwitchProfile(this.clientProfileId);
      }
    });

  }

  OnGet(){

  }
  onProfileChange(eventData)
  {
    this.router.navigate(['/secured/dashboards/client-view/'+eventData.value]); 
  }

  OnSwitchProfile(id)
  {
    this.clientProfileId = id;
    this.showDetail = true;
    if (this.clientProfileSummaryChild)
    {
      this.clientProfileSummaryChild.OnLoad(this.clientProfileId);
      this.clientProfileServicesChild.OnLoad(this.clientProfileId);
    }
  }

  OnServices(serviceCode:string){
    if (this.showDetail)
      this.router.navigate(['secured/services/registration/'+serviceCode+'/'+this.clientProfileId]); 
  }
  OnRegister(serviceCode:string){
    if (this.showDetail)
      this.router.navigate(['secured/registers/'+serviceCode+'/'+this.clientProfileId]); 
  }
  OnDownload(id)
  {
    if (this.showDetail)
    {
      if(id===1)
      {
          const dialogRef = this.dialog.open(UploadComponent, {
            width: '600px',
            data:{action:'Signing',data:{serviceBusinessId:this.clientProfileId,officerId:0,serviceName:'General Upload'}},
            disableClose: true 
          });
      }
      else if (id===2)
      {
        this.router.navigate(['secured/reports/uploaded-documents/'+this.clientProfileId+'/'+this.clientProfileSummaryChild.profileName]);    
      } 
      else if (id===3)
      {
        this.router.navigate(['secured/reports/signed-documents/'+this.clientProfileId+'/'+this.clientProfileSummaryChild.profileName]);    
      } 
    }
      
  }
}
 