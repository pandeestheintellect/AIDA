import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceDefinitionDetailComponent } from './service-definition-detail.component';

describe('ServiceDefinitionDetailComponent', () => {
  let component: ServiceDefinitionDetailComponent;
  let fixture: ComponentFixture<ServiceDefinitionDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServiceDefinitionDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServiceDefinitionDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
