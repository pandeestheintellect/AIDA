import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';
import { ResourceClientService } from 'src/app/service/resourceclient.service';
import { ResponseModel } from 'src/app/shared/models/common-data';

@Component({
  selector: 'app-invite',
  templateUrl: './invite.component.html',
  styleUrls: ['../../login/login.component.scss']
})
export class InviteComponent implements OnInit {

  username: string;
  password:string;
  @Input() notes: string | null;
  key:string='';
  serviceBusinessId=0;
  officerId=0;
  constructor(private resourceClient: ResourceClientService,private route: ActivatedRoute,private router: Router,
    private auth: AuthService) { }

  ngOnInit(): void {
    
    this.route.params.subscribe(params => {
      this.key = params['key'];
      if (this.key !==undefined)
      {
        this.resourceClient.getDataInGet( 'api/service-execution-document-otp/'+this.key).subscribe((data:ResponseModel)=>{
          this.notes=data.message;
          
        });
      }
    });
    
    this.notes='Please wait sending OTP ...';
  }

  Onlogin() {

    
    if (this.password.length>5)
    {
      this.resourceClient.getDataInPost( 'api/service-execution-document-otp',{InviteKey:this.key,OTP:this.password}).subscribe((data:any)=>{
        if (data.isSuccess)
        {
          this.auth.doFormFilling (data.serviceBusinessId,data.officerId);
        }
        else
          this.notes=data.message;
      });
    }
    
    
  }
}