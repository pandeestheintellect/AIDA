import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControllersCorporateDetailComponent } from './controllers-corporate-detail.component';

describe('ControllersCorporateDetailComponent', () => {
  let component: ControllersCorporateDetailComponent;
  let fixture: ComponentFixture<ControllersCorporateDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControllersCorporateDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControllersCorporateDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
