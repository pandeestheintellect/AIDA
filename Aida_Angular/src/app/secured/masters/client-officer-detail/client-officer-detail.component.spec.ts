import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientOfficerDetailComponent } from './client-officer-detail.component';

describe('ClientOfficerDetailComponent', () => {
  let component: ClientOfficerDetailComponent;
  let fixture: ComponentFixture<ClientOfficerDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientOfficerDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientOfficerDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
