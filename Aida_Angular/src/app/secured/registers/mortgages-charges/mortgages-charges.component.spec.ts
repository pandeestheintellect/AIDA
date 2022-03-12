import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MortgagesChargesComponent } from './mortgages-charges.component';

describe('MortgagesChargesComponent', () => {
  let component: MortgagesChargesComponent;
  let fixture: ComponentFixture<MortgagesChargesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MortgagesChargesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MortgagesChargesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
