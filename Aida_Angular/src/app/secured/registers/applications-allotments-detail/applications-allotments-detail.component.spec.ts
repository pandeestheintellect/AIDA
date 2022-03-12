import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationsAllotmentsDetailComponent } from './applications-allotments-detail.component';

describe('ApplicationsAllotmentsDetailComponent', () => {
  let component: ApplicationsAllotmentsDetailComponent;
  let fixture: ComponentFixture<ApplicationsAllotmentsDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApplicationsAllotmentsDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationsAllotmentsDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
