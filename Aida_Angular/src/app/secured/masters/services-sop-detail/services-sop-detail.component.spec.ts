import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServicesSopDetailComponent } from './services-sop-detail.component';

describe('ServicesSopDetailComponent', () => {
  let component: ServicesSopDetailComponent;
  let fixture: ComponentFixture<ServicesSopDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServicesSopDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServicesSopDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
