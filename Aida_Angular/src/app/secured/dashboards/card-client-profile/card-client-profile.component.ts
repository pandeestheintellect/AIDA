import { Component, OnInit,Input } from '@angular/core';
import {MatTableDataSource} from '@angular/material/table';
import { Router} from '@angular/router';

import {ClientProfileModel} from '../../../shared/models/client-profile-data';
import {ResourceClientService } from '../../../service/resourceclient.service';


@Component({
  selector: 'app-card-client-profile',
  template: `
    
    <div fxLayout="row wrap" class="edit-section">
          <div fxFlex="auto" fxFlex.gt-sm="98%" style="padding: 20px;">
            
            <h2>{{profileData.name}} - {{profileData.clientType}} </h2> 
            <b>{{address}}</b>
           
          </div>
          <div fxFlex="auto" fxFlex.gt-sm="30%" style="padding: 20px;margin-top: -25px !important;">
                <b>UEN: </b>{{profileData.uen}}
                <br><b>Incopration Date: </b>{{profileData.incorpDate}}
            </div>
            <div fxFlex="auto" fxFlex.gt-sm="30%" style="padding: 20px;margin-top: -25px !important;">
              <b>Issued Share Capital: </b> {{profileData.issuedCapital}}
              <br><b>Number of Shares: </b> {{profileData.issuedShares}}       </div>
            <div fxFlex="auto" fxFlex.gt-sm="30%" style="padding: 20px;margin-top: -25px !important;">
     
              <b>Paid-up Capital: </b> {{profileData.paidupCapital}}
              <br><b>Number of Shares: </b> {{profileData.paidupShares}}
               
            </div>
            <div fxFlex="auto" fxFlex.gt-sm="10%" style="padding: 20px;text-align: center;margin-top: -25px !important;">
              <h2>{{profileData.status}}</h2> 
            </div>

        </div>

  `,
  styles: []
})
export class CardClientProfileComponent implements OnInit {

  @Input()  clientProfileId: string;
  
  profileData={name: '',uen: '',incorpDate: null,address1: '',address2:'',city: '',country: '',clientType:'PRIVATE LIMITED COMPANY',
  pincode: '',mobile: '',email:'',industryType: '',status: '',statusDate: null,tradingName:'',phone:'',nature:'',
  issuedCapital:0,issuedShares:0,paidupCapital:0,paidupShares:0};

  address:string='';
  
  profileName:string='';

  constructor(private resourceClient: ResourceClientService) { }

  ngOnInit(): void {

      if (this.clientProfileId!=='0')
        this.OnLoad(this.clientProfileId);

  }

  public OnLoad(clientProfileId:any)
  {
    //alert(clientProfileId);
    this.resourceClient.getDataInGet('api/business-profile/'+clientProfileId).subscribe((data: ClientProfileModel)=>{
      this.profileData = data;
      this.address=data.address1+', '+data.city+', '+data.country+', Postal Code - '+ data.pincode
        +', Phone : '+ data.phone +', Mobile: '+data.mobile +', Email: '+data.email;

      this.profileName = data.name;

    })  
  }
}
