import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControllersCorporateComponent } from './controllers-corporate.component';

describe('ControllersCorporateComponent', () => {
  let component: ControllersCorporateComponent;
  let fixture: ComponentFixture<ControllersCorporateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControllersCorporateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControllersCorporateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
