import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControllersIndividualsComponent } from './controllers-individuals.component';

describe('ControllersIndividualsComponent', () => {
  let component: ControllersIndividualsComponent;
  let fixture: ComponentFixture<ControllersIndividualsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControllersIndividualsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControllersIndividualsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
