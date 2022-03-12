import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectorsShareholdingsComponent } from './directors-shareholdings.component';

describe('DirectorsShareholdingsComponent', () => {
  let component: DirectorsShareholdingsComponent;
  let fixture: ComponentFixture<DirectorsShareholdingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DirectorsShareholdingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DirectorsShareholdingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
