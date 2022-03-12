import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagingDirectorsComponent } from './managing-directors.component';

describe('ManagingDirectorsComponent', () => {
  let component: ManagingDirectorsComponent;
  let fixture: ComponentFixture<ManagingDirectorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagingDirectorsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagingDirectorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
