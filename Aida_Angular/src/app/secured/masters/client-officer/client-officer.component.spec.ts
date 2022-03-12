import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientOfficerComponent } from './client-officer.component';

describe('ClientOfficerComponent', () => {
  let component: ClientOfficerComponent;
  let fixture: ComponentFixture<ClientOfficerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientOfficerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientOfficerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
