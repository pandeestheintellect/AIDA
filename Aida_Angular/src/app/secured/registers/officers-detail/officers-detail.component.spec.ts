import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfficersDetailComponent } from './officers-detail.component';

describe('OfficersDetailComponent', () => {
  let component: OfficersDetailComponent;
  let fixture: ComponentFixture<OfficersDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfficersDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfficersDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
