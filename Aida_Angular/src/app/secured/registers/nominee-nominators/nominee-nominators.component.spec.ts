import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NomineeNominatorsComponent } from './nominee-nominators.component';

describe('NomineeNominatorsComponent', () => {
  let component: NomineeNominatorsComponent;
  let fixture: ComponentFixture<NomineeNominatorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NomineeNominatorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NomineeNominatorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
