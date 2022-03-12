import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NomineeNominatorsDetailComponent } from './nominee-nominators-detail.component';

describe('NomineeNominatorsDetailComponent', () => {
  let component: NomineeNominatorsDetailComponent;
  let fixture: ComponentFixture<NomineeNominatorsDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NomineeNominatorsDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NomineeNominatorsDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
