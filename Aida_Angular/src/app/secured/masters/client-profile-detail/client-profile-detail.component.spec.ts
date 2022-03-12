import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientProfileDetailComponent } from './client-profile-detail.component';

describe('ClientProfileDetailComponent', () => {
  let component: ClientProfileDetailComponent;
  let fixture: ComponentFixture<ClientProfileDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientProfileDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientProfileDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
