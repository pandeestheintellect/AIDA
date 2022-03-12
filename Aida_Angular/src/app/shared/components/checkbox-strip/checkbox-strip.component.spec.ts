import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckboxStripComponent } from './checkbox-strip.component';

describe('CheckboxStripComponent', () => {
  let component: CheckboxStripComponent;
  let fixture: ComponentFixture<CheckboxStripComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckboxStripComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckboxStripComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
