import { Component, OnInit,Input } from '@angular/core';
import {ActivatedRoute,Router} from '@angular/router';
import { ResourceClientService } from '../service/resourceclient.service';

import { AuthService } from '../service/auth.service';


@Component({
  selector: 'app-document-invite',
  templateUrl: './document-invite.component.html',
  styleUrls: ['../login/login.component.scss']
})
export class DocumentInviteComponent implements OnInit {

  username: string;
  password:string;
  @Input() error: string | null;
  key:string='';
  serviceBusinessId=0;
  officerId=0;
  constructor(private resourceClient: ResourceClientService,private route: ActivatedRoute,
    private auth: AuthService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.key = params['key'];
      if (this.key !==undefined)
      {
        var keys = atob(this.key).split(',');
        //this.auth.doFormFilling (keys[1],keys[3]);
      }
    });
  }

  Onlogin() {
    
  }
}
