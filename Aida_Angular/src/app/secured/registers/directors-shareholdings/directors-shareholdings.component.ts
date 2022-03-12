import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ResourceClientService } from 'src/app/service/resourceclient.service';
import { DropDownModel } from 'src/app/shared/models/common-data';
import { MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-directors-shareholdings',
  templateUrl: './directors-shareholdings.component.html',
  styles: []
})
export class DirectorsShareholdingsComponent implements OnInit {

  pageTitle = "Register of Directors’ Shareholdings";
  clientProfileId='0';
  officerId='0';

  companyNameList: DropDownModel[] = [];
  officerList: DropDownModel[] = [];
  
  rootPath="api/register/applications-allotments";

  constructor(private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private snackBar: MatSnackBar) { }

    ngOnInit(): void {
      this.resourceClient.getDataInGet('api/masters/dropdown/business-profile').subscribe((data: DropDownModel[])=>{
        this.companyNameList = data;
      });
  
      this.route.params.subscribe(params => {
        this.clientProfileId = params['clientProfileId'] ;
        this.onLoadOfficer();
      });
  
      
    }
    onLoadOfficer()
    {


      this.resourceClient.getDataInGet('api/business-officers-dropdown/'+this.clientProfileId).subscribe((data: DropDownModel[])=>{
        this.officerList = data;
      });
  
    }

    OnShowMessage(message)
  {
    this.snackBar.open(message,'', {duration: 2000,verticalPosition: 'top',panelClass: 'snackBarBackgroundColor'});
  }

  OnPrint()
  {
    if (this.officerId==='0')
    {
      this.OnShowMessage('Please choose an officer to print');
      return;
    }

    this.resourceClient.getDataInGet('api/register/download-shareholdings-registers'+'/'+this.clientProfileId+'/'+this.officerId).subscribe((data: any)=>{
      
      window.open(this.resourceClient.REST_API_SERVER + 'api/register-download/'+data,'_blank') ;

    });
  }
} 
