import { Component, OnInit } from '@angular/core';
import {TableData} from '../../../shared/components/card-table/card-table.data';

@Component({
  selector: 'app-company-view',
  templateUrl: './company-view.component.html',
  styles: []
})
export class CompanyViewComponent implements OnInit {

  registraionInProgress:string = 'In-Progress';
  registraionCompleted:string = 'Completed';
  registraionTerminated:string = 'Terminated';

  serviceStarted:string = 'Started';
  serviceRunning:string = 'Ongoing';
  serviceFinished:string = 'Finished';

  
  constructor() { }

  ngOnInit(): void {
  }

}
