import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectorsShareholdingsDetailComponent } from './directors-shareholdings-detail.component';

describe('DirectorsShareholdingsDetailComponent', () => {
  let component: DirectorsShareholdingsDetailComponent;
  let fixture: ComponentFixture<DirectorsShareholdingsDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DirectorsShareholdingsDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DirectorsShareholdingsDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
