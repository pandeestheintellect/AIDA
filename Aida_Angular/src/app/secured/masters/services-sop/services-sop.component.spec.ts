import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServicesSopComponent } from './services-sop.component';

describe('ServicesSopComponent', () => {
  let component: ServicesSopComponent;
  let fixture: ComponentFixture<ServicesSopComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServicesSopComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServicesSopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
