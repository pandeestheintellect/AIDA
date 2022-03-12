import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagingDirectorsDetailComponent } from './managing-directors-detail.component';

describe('ManagingDirectorsDetailComponent', () => {
  let component: ManagingDirectorsDetailComponent;
  let fixture: ComponentFixture<ManagingDirectorsDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagingDirectorsDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagingDirectorsDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
