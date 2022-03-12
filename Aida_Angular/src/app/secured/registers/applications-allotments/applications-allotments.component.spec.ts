import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationsAllotmentsComponent } from './applications-allotments.component';

describe('ApplicationsAllotmentsComponent', () => {
  let component: ApplicationsAllotmentsComponent;
  let fixture: ComponentFixture<ApplicationsAllotmentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApplicationsAllotmentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationsAllotmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
