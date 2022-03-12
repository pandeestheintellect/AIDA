import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceDefinitionComponent } from './service-definition.component';

describe('ServiceDefinitionComponent', () => {
  let component: ServiceDefinitionComponent;
  let fixture: ComponentFixture<ServiceDefinitionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServiceDefinitionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServiceDefinitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
