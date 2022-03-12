import { Component, OnInit ,ViewChild} from '@angular/core';
import {ActivatedRoute,Router} from '@angular/router';
import { HttpUrlEncodingCodec } from '@angular/common/http';
import { ResourceClientService } from '../service/resourceclient.service';

import { DropDownModel } from '../shared/models/common-data'


@Component({
  selector: 'app-myinfo-callback',
  templateUrl: './myinfo-callback.component.html',
  styleUrls: ['./myinfo-callback.component.scss']
})
export class MyinfoCallbackComponent implements OnInit {

  
  codec :HttpUrlEncodingCodec = new HttpUrlEncodingCodec;

  code:string='';
  state:string='';
  myinfoData:DropDownModel[]=[];
  dataSubmited=false;

  constructor(private route: ActivatedRoute,private resourceClient: ResourceClientService) { }

  ngOnInit(): void {

    this.code = this.route.snapshot.queryParams['code'];
    this.state = this.route.snapshot.queryParams['state'];

    this.myinfoData.push({value:'........',text:'Loading'});
    this.OnToken(); 
  }

  OnToken()
  {
    this.resourceClient.getDataInGet('api/myinfo-person?code='+this.code + '&state='+this.state).subscribe((data:DropDownModel[])=>{
      this.myinfoData = data;
    }) 

  }
  OnOK() {
    this.dataSubmited=true;
    this.resourceClient.getDataInGet('api/myinfo-person/'+this.state).subscribe((data:DropDownModel[])=>{
      this.myinfoData = data;
    }) 
    
   
  }
  
  OnClose() {
    window.close();
  }
  
}
