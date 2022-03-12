import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BusinessMyinfoIntroComponent } from './business-myinfo-intro.component';

describe('BusinessMyinfoIntroComponent', () => {
  let component: BusinessMyinfoIntroComponent;
  let fixture: ComponentFixture<BusinessMyinfoIntroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BusinessMyinfoIntroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BusinessMyinfoIntroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
