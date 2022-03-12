import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditorsDetailComponent } from './auditors-detail.component';

describe('AuditorsDetailComponent', () => {
  let component: AuditorsDetailComponent;
  let fixture: ComponentFixture<AuditorsDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditorsDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditorsDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
