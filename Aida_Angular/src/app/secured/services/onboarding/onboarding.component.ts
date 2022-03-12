import { Component, OnInit, ViewChild } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog'
import {Router} from '@angular/router';
import { HttpUrlEncodingCodec } from '@angular/common/http'

import { ResourceClientService } from '../../../service/resourceclient.service';
import { ServicesDefinitionModel } from '../../../shared/models/service-data';

@Component({
  selector: 'app-onboarding',
  templateUrl: './onboarding.component.html',
  styles: []
})
export class OnboardingComponent implements OnInit {

  pageTitle='Service Onboarding';
  rootPath="api/service-definitions";
  displayedColumns: string[] = ['code','name','remarks','toolbox'];
  
  dataSource = new MatTableDataSource<ServicesDefinitionModel>();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  constructor(public dialog: MatDialog,private resourceClient: ResourceClientService,private router: Router) { }

  ngOnInit() {
    this.resourceClient.getDataInGet(this.rootPath).subscribe((data: any[])=>{
      this.dataSource = new MatTableDataSource<ServicesDefinitionModel>(data);
      this.dataSource.paginator = this.paginator;
    })  
    
  }

  OnRegister(data) {
    this.router.navigate(['secured/services/registration',data.code,data.name]);
  }
  OnListing(data) {
    this.router.navigate(['secured/services/listing',data.code]);
  }
  OnExport(exportType){

  }
}