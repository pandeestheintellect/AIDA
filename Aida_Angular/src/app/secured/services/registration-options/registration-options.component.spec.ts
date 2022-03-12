import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationOptionsComponent } from './registration-options.component';

describe('RegistrationOptionsComponent', () => {
  let component: RegistrationOptionsComponent;
  let fixture: ComponentFixture<RegistrationOptionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrationOptionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrationOptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
