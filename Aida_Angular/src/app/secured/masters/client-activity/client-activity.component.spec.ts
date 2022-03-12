import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BusinessActivityComponent } from './business-activity.component';

describe('BusinessActivityComponent', () => {
  let component: BusinessActivityComponent;
  let fixture: ComponentFixture<BusinessActivityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BusinessActivityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BusinessActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
