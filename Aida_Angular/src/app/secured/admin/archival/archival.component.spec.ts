import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArchivalComponent } from './archival.component';

describe('ArchivalComponent', () => {
  let component: ArchivalComponent;
  let fixture: ComponentFixture<ArchivalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArchivalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArchivalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
