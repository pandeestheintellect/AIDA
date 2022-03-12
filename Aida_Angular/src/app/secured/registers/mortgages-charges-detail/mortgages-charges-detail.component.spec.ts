import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MortgagesChargesDetailComponent } from './mortgages-charges-detail.component';

describe('MortgagesChargesDetailComponent', () => {
  let component: MortgagesChargesDetailComponent;
  let fixture: ComponentFixture<MortgagesChargesDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MortgagesChargesDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MortgagesChargesDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
