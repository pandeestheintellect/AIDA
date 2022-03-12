import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmploymentAgencyComponent } from './employment-agency.component';

describe('EmploymentAgencyComponent', () => {
  let component: EmploymentAgencyComponent;
  let fixture: ComponentFixture<EmploymentAgencyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmploymentAgencyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmploymentAgencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
